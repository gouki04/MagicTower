using System.Collections;
using System.Collections.Generic;

namespace SafeCoroutine
{
    public class CoroutineManager
    {
        private static List<Coroutine> mCoroutines = new List<Coroutine>();
        private static List<Coroutine> mSalvageCoroutines = new List<Coroutine>();

        private static bool mIsUpdating = false;

        public static void Update(float delta_time)
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

        public static Coroutine StartCoroutine(IEnumerator iterator)
        {
            if (iterator == null)
                return null;

            var c = new Coroutine(iterator);

            if (mIsUpdating)
                mSalvageCoroutines.Add(c);
            else
                mCoroutines.Add(c);

            return c;
        }
    }
}
