using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeCoroutine
{
    public interface IYieldInstruction
    {
        bool IsComplete(float delta_time);
    }
}
