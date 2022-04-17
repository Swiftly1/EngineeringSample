using System;
using System.Collections.Generic;

namespace Emitter.LLVM
{
    public class LLVM_ScopeInfo
    {
        public LLVM_ScopeInfo()
        {
            InstanceNumber = InstanceCounter++;
        }

        private static int InstanceCounter = 0;

        public int InstanceNumber { get; set; }

        /// <summary>
        /// Code Variable Name, LLVM IR Variable Id
        /// </summary>
        public Dictionary<string, int> VariablesWithinScope { get; set; } = new();

        private int VariableCounter = 1;

        private int TrueBranchCounter = 1;

        private int FalseBranchCounter = 1;

        private int EndBranchCounter = 1;

        private int ArgsCounter = 0;

        public int GetNextVariableNumber()
        {
            if (ArgsCounter == 0)
            {
                return VariableCounter++;
            }
            else
            {
                var current = VariableCounter;
                VariableCounter++;
                return current + ArgsCounter;
            }
        }

        // TODO: Shouldn't those counters be global?
        public int GetNextTrueBranchNumber() => TrueBranchCounter++;

        public int GetNextFalseBranchNumber() => FalseBranchCounter++;

        public int GetNextEndBranchNumber() => EndBranchCounter++;

        public void RegisterFunctionArg(int args_count)
        {
            ArgsCounter = args_count;
        }
    }
}