
namespace SafeCoroutine
{
    public class WaitForSeconds : IYieldInstruction
    {
        private float mTimeDelay;

        public WaitForSeconds(float delay)
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
}
