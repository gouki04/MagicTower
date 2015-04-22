
namespace SafeCoroutine
{
    public interface IYieldInstruction
    {
        bool IsComplete(float delta_time);
    }
}
