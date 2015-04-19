using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeCoroutine
{
    public class WaitForAll : IYieldInstruction
    {
        private IYieldInstruction[] mInstructions;

        public WaitForAll(params object[] instructions)
        {
            mInstructions = new IYieldInstruction[instructions.Length];
            for (int i = 0; i < instructions.Length; ++i)
            {
                mInstructions[i] = instructions[i] as IYieldInstruction;
            }
        }

        public bool IsComplete(float delta_time)
        {
            bool anyRunning = false;
            for (int i = 0; i < mInstructions.Length; ++i)
            {
                var instruction = mInstructions[i];
                if (instruction != null)
                { 
                    if (false && instruction is Coroutine)
                    {
                        var coroutine = instruction as Coroutine;
                        if (coroutine.IsFinish)
                            mInstructions[i] = null;
                        else
                            anyRunning = true;
                    }
                    else
                    {
                        var result = instruction.IsComplete(delta_time);
                        if (result == true)
                            mInstructions[i] = null;
                        else
                            anyRunning = true;
                    }
                }
            }

            return !anyRunning;
        }
    }
}
