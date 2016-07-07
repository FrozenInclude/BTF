using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BTF
{
    public class PyParser : InterPreter
    {
        private int ptrsize;
        private int plusCounter = 0;
        private int nested = 0;
        private string 들여쓰기 = "";
        private string last들여쓰기 = "";
        private int minusCounter = 0;
        private int plusCounters = 0;
        private int minusCounters = 0;
        private int loop { get; set; }
        private string command;
        public PyParser(string code, int ptrsize) : base(code, ptrsize)
        {
            this.ptrsize = ptrsize;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Action(Opcode command)
        {
            if (command == Opcode.DecreasePointer)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounter++;
            }
            else if (command == Opcode.IncreasePointer)
            {
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                plusCounter++;
            }
            else if (command == Opcode.IncreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounter + Environment.NewLine}";
                    minusCounters = 0;
                }
                plusCounters++;
                //output +="ptr[memory]++;\n";
            }
            else if (command == Opcode.DecreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounters++;
            }
            else if (command == Opcode.Output)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                output +=$"{들여쓰기}print(chr(ptr[memory]));\n";
            }
            else if (command == Opcode.Openloop)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                nested++;
                if (nested == 1)
                {
                    output += $"while ptr[memory] != 0 :{Environment.NewLine}";
                    last들여쓰기 = 들여쓰기;
                }
                else if(nested>=1)
                {
                    output += $"{들여쓰기}while ptr[memory] != 0 :{Environment.NewLine}";
                    들여쓰기 += "       ";
                }
                   들여쓰기 += "                     ";
            }
            else if (command == Opcode.Input)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $@"{들여쓰기}ptr[memory]=input()";
            }
            if (command == Opcode.Closeloop)
            {
                if (plusCounter > 0)
                {
                    output +=$"{들여쓰기}memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output +=$"{들여쓰기}memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output +=$"{들여쓰기}ptr[memory]+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
                nested--;
               if (nested == 1) 
                들여쓰기 = "                     " + last들여쓰기;
               else if (nested == 0)
                {
                    들여쓰기 = "";
                }
            }
            else if (command == Opcode.Result)
            {
                if (plusCounter > 0)
                {
                    output += $"          memory+={plusCounter + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          memory-={minusCounter + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          ptr(memory)-={minusCounters + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          ptr(memory)+={plusCounters + Environment.NewLine}";
                    plusCounters = 0;
                }
            }
        }

        public override void RunCode()
        {
            command = code;
            nested = 0;
            last들여쓰기 = "";
            if (code != null)
            {
                while (loop < code.Length)
                {
                    try
                    {
                        switch (command[loop])
                        {
                            case (char)Opcode.DecreasePointer:
                                Action(Opcode.DecreasePointer);
                                if (loop == code.Length)
                                    Action(Opcode.Result);
                                //  output += "--memory;\n";
                                break;
                            case (char)Opcode.IncreasePointer://>
                                Action(Opcode.IncreasePointer);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.IncreaseDataPointer://+    
                                Action(Opcode.IncreaseDataPointer);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                //output +="ptr[memory]++;\n";
                                break;
                            case (char)Opcode.DecreaseDataPointer://-  
                                Action(Opcode.DecreaseDataPointer);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                // output +=" *ptr--;\n";
                                break;
                            case (char)Opcode.Output:
                                Action(Opcode.Output);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.Input:
                                Action(Opcode.Input);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.Openloop:
                                Action(Opcode.Openloop);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.Closeloop:
                                Action(Opcode.Closeloop);
                                if (loop == code.Length + 1)
                                    Action(Opcode.Result);
                                break;
                        }
                        loop++;
                    }
                    catch (Exception E)
                    {
                        output = "Overflow Error!!";
                        return;
                    }
                }
                output = $@"ptr=[0]*{ptrsize};
memory=0;
{output}";
            }
        }
    }
}


