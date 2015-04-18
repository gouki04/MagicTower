using UnityEngine;
using System.Collections;

/// <summary>
/// yield指令
/// </summary>
public interface ISafeYieldInstruction
{
    bool IsComplete(float delta_time);
}

/// <summary>
/// 安全的协程
/// </summary>
public class SafeCoroutine : ISafeYieldInstruction
{
	public enum EState
	{
		NotInit = 0,
		Stopped = 1,
		Running = 2,
		Paused = 3,
		SelfPause = 4,
		ParentPause = 5,
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
		get { return mState == EState.Paused || IsParentPaused; }
	}

	public bool IsSelfPaused
	{
		get { return mState == EState.Paused; }
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
			mState = EState.Paused;
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

		if (IsPause || IsParentPaused)
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
