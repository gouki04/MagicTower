using System.Collections;

namespace SafeCoroutine
{
    /// <summary>
    /// 安全的协程
    /// </summary>
    public class Coroutine
    {
        /// <summary>
        /// 协程的状态
        /// </summary>
        public enum EState
        {
            NotInit = 0, // 未初始化
            Stopped = 1, // 完成了或停止了
            Running = 2, // 运行中（但有可能因为父亲被暂停而无法运行）
            Paused = 3,  // 暂停了
        }

        /// <summary>
        /// 协程
        /// C#中的协程就是用IEnumerator实现的
        /// </summary>
        protected IEnumerator mIterator;

        /// <summary>
        /// 孩子协程
        /// 在自身运行中开启的子协程
        /// </summary>
        protected Coroutine mChildCoroutine;

        /// <summary>
        /// 父亲协程
        /// </summary>
        protected Coroutine mParentCoroutine;

        /// <summary>
        /// 当前状态
        /// </summary>
        protected EState mState = EState.NotInit;
        public EState State
        {
            get { return mState; }
        }

        /// <summary>
        /// 协程的返回值
        /// </summary>
        protected object mResult;
        public object Result
        {
            get { return mResult; }
        }

        /// <summary>
        /// 是否在运行状态（父亲被暂停也返回true）
        /// </summary>
        public bool IsRunning
        {
            get { return mState == EState.Running; }
        }

        /// <summary>
        /// 是否在stop状态
        /// </summary>
        public bool IsFinish
        {
            get { return mState == EState.Stopped; }
        }

        /// <summary>
        /// 是否在暂停中
        /// 自己被暂停或者父亲被暂停都返回true
        /// </summary>
        public bool IsPause
        {
            get { return mState == EState.Paused || IsParentPaused; }
        }

        /// <summary>
        /// 是否自己被暂停
        /// </summary>
        public bool IsSelfPaused
        {
            get { return mState == EState.Paused; }
        }

        /// <summary>
        /// 父亲是否在暂停中
        /// </summary>
        protected bool IsParentPaused
        {
            get
            {
                if (mParentCoroutine == null)
                    return false;

                return mParentCoroutine.IsPause;
            }
        }

        public Coroutine(IEnumerator itr)
        {
            mIterator = itr;

            if (mIterator != null)
            {
                mState = EState.Running;
                mIterator.MoveNext();
            }
            else
            {
                mState = EState.Stopped;
            }
        }

        /// <summary>
        /// 协程完成了
        /// </summary>
        /// <returns>只返回true</returns>
        private bool finish()
        {
            mState = EState.Stopped;
            return true;
        }

        /// <summary>
        /// 暂停协程
        /// </summary>
        public void Pause()
        {
            if (mState == EState.Running)
            {
                mState = EState.Paused;
            }
            else
            {
                logStateError("Pause");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function_name"></param>
        protected void logStateError(string function_name)
        {
            System.Console.WriteLine("Coroutine." + function_name + " error! state = " + System.Enum.GetName(typeof(EState), mState));
        }

        /// <summary>
        /// 恢复协程
        /// </summary>
        public void Resume()
        {
            if (mState == EState.Paused)
            {
                mState = EState.Running;
            }
            else
            {
                logStateError("Resume");
            }
        }

        /// <summary>
        /// 停止协程
        /// 会把孩子协程也停止了
        /// </summary>
        public void Stop()
        {
            mIterator = null;
            mState = EState.Stopped;

            if (mChildCoroutine != null)
            {
                mChildCoroutine.Stop();
            }
        }

        /// <summary>
        /// 是否完成了
        /// 每帧调用
        /// </summary>
        /// <param name="delta_time"></param>
        /// <returns>完成了返回true</returns>
        public bool IsComplete(float delta_time)
        {
            if (mIterator == null)
                return finish();

            if (IsPause)
                return false;

            if (mIterator.Current == null)
            {
                if (!mIterator.MoveNext())
                    return finish();
            }
            else if (mIterator.Current is Coroutine)
            {
                // 当前执行的是子协程
                // 子协程不需要在这里更新，它会在CoroutineController里更新
                if (mChildCoroutine == null)
                {
                    // 第一次执行，设置父子关系
                    mChildCoroutine = mIterator.Current as Coroutine;
                    mChildCoroutine.mParentCoroutine = this;
                }
                else if (mChildCoroutine.IsFinish)
                {
                    // 如果已经完成了，执行下一步
                    mChildCoroutine = null;
                    if (!mIterator.MoveNext())
                        return finish();
                }
            }
            else if (mIterator.Current is IYieldInstruction)
            {
                // 当前执行的是普通的yield指令
                var yieldBase = mIterator.Current as IYieldInstruction;

                // 更新指令
                bool result = yieldBase.IsComplete(delta_time);
                if (result && !mIterator.MoveNext())
                    return finish();
            }
            else
            {
                if (!mIterator.MoveNext())
                {
                    // 已经没有下一步了，设置完成状态
                    mResult = mIterator.Current;
                    return finish();
                }
            }

            return false;
        }
    }
}
