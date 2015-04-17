using UnityEngine;
using System.Collections;

public interface ISafeYieldInstruction
{
    bool IsComplete(float delta_time);
}

public class SafeCoroutine : ISafeYieldInstruction
{
	public enum EState
	{
		NotInit = 0,
		Stopped = 1,
		Running = 2,
		SelfPause = 3,
		ParentPause = 4,
	}

    private IEnumerator mIterator;

	private SafeCoroutine mChildCoroutine;
	private SafeCoroutine mParentCoroutine;

	private EState mState = EState.NotInit;
	public EState State
	{
		get { return mState; }
	}

	private object mResult;
	public object Result
	{
		get { return mResult; }
	}

	public bool IsRunning
	{
		get { return mState == EState.Running; }
	}

	public bool IsFinish
	{
		get { return mState == EState.Stopped; }
	}

	public bool IsPause
	{
		get { return mState == EState.SelfPause || mState == EState.ParentPause; }
	}

	public bool CanPause
	{
		get { return mState == EState.Running || mState == EState.ParentPause; }
	}

	protected bool IsParentPaused
	{
		get
		{
			if (mParentCoroutine == null)
				return false;

			return mParentCoroutine.IsPause;
		}
	}

    public SafeCoroutine(IEnumerator itr)
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

	private bool finish()
	{
		mState = EState.Stopped;
		return true;
	}

	public void Pause()
	{
		if (mState == EState.Running)
		{
			mState = EState.SelfPause;

			if (mChildCoroutine != null)
			{
				mChildCoroutine.pauseByParent();
			}
		}
		else if (mState == EState.ParentPause)
		{
			mState = EState.SelfPause;
		}
		else
		{
			logStateError("Pause");
		}
	}

	protected void logStateError(string function_name)
	{
		Debug.LogWarning("Coroutine." + function_name + " error! state = " + System.Enum.GetName(typeof(EState), mState));
	}

	protected void resumeFromParent()
	{
		if (mState == EState.ParentPause)
		{
			mState = EState.Running;

			if (mChildCoroutine != null)
				mChildCoroutine.resumeFromParent();
		}
		else if (mState == EState.SelfPause)
		{
			// 不用处理，维持在selfPause
		}
		else
		{
			logStateError("resumeFromParent");
		}
	}

	public void Resume()
	{
		if (mState == EState.SelfPause)
		{
			mState = IsParentPaused ? EState.ParentPause : EState.Running;

			if (mState == EState.Running && mChildCoroutine != null)
				mChildCoroutine.resumeFromParent();
		}
		else
		{
			logStateError("Resume");
		}
	}

	protected void pauseByParent()
	{
		if (mState == EState.Running)
		{
			mState = EState.ParentPause;

			if (mChildCoroutine != null)
				mChildCoroutine.pauseByParent();
		}
		else if (mState == EState.SelfPause)
		{
			// 不用处理，维持在selfPause
		}
		else
		{
			logStateError("pauseByParent");
		}
	}

	public void Stop()
	{
		mIterator = null;
		mState = EState.Stopped;

		if (mChildCoroutine != null)
		{
			mChildCoroutine.Stop();
		}
	}

    public bool IsComplete(float delta_time)
    {
        if (mIterator == null)
            return finish();

		if (IsPause == true)
			return false;

        if (mIterator.Current == null)
        {
            if (!mIterator.MoveNext())
				return finish();
        }
		else if ((mIterator.Current as SafeCoroutine) != null)
		{
			var child = mIterator.Current as SafeCoroutine;
			if (mChildCoroutine == null)
			{
				mChildCoroutine = child;
				child.mParentCoroutine = this;
			}
			else if (mChildCoroutine.IsFinish)
			{
				mChildCoroutine = null;
				if (!mIterator.MoveNext())
					return finish();
			}
		}
		else if ((mIterator.Current as ISafeYieldInstruction) != null)
		{
			ISafeYieldInstruction yieldBase = mIterator.Current as ISafeYieldInstruction;
			bool result = yieldBase.IsComplete(delta_time);
			if (result && !mIterator.MoveNext())
				return finish();
		}
        else
        {
            if (!mIterator.MoveNext())
			{
				mResult = mIterator.Current;
				return finish();
			}
		}

        return false;
    }
}

public class SafeWaitForSeconds : ISafeYieldInstruction
{
    private float mTimeDelay;

    public SafeWaitForSeconds(float delay)
    {
        mTimeDelay = delay;
    }

    public bool IsComplete(float delta_time)
    {
        if (mTimeDelay < 0)
            return true;
        mTimeDelay -= delta_time;
        return false;
    }
}

public class SafeWaitAnimEnd : ISafeYieldInstruction
{
    private Animation mAnim;
    private string mAnimName;

    public SafeWaitAnimEnd(Animation anim, string name)
    {
        mAnim = anim;
        mAnimName = name;
    }

    public bool IsComplete(float delta_time)
    {
        return !mAnim.IsPlaying(mAnimName);
    }
}
