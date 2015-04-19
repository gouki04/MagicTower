using System.Collections;
using System.Collections.Generic;

public class CoroutineController : Singleton<CoroutineController> 
{
    List<SafeCoroutine> mCoroutines = new List<SafeCoroutine>();
	List<SafeCoroutine> mSalvageCoroutines = new List<SafeCoroutine>();

	private bool mIsUpdating = false;

    public void Update(float delta_time)
    {
		mIsUpdating = true;
        for (int i = mCoroutines.Count - 1; i >= 0; --i)
        {
            if (mCoroutines[i].IsComplete(delta_time))
                mCoroutines.RemoveAt(i);
        }
		mIsUpdating = false;

		if (mSalvageCoroutines.Count > 0)
		{
			mCoroutines.AddRange(mSalvageCoroutines);
			mSalvageCoroutines.Clear();
		}
	}
	
    public void StopSafeCoroutine(SafeCoroutine c)
    {
        if (c == null)
            return;

        mCoroutines.Remove(c);
    }

    public SafeCoroutine StartSafeCoroutine(IEnumerator iterator)
    {
        if (iterator == null)
            return null;

        SafeCoroutine c = new SafeCoroutine(iterator);

		if (mIsUpdating)
			mSalvageCoroutines.Add(c);
		else
	        mCoroutines.Add(c);

        return c;
    }
}
