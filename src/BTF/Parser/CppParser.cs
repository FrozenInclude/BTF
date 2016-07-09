using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BTF
{
    public class CppParser : InterPreter
    {
        private int ptrsize;
        private int plusCounter = 0;
        private int minusCounter = 0;
        private int plusCounters = 0;
        private int minusCounters = 0;
        private int loop { get; set; }
        private string command;
        public CppParser(string code, int ptrsize) : base(code, ptrsize)
        {
            this.ptrsize = ptrsize;
        }
        public object getPtrValue
        {
            get
            {
                return base.ptr;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Action(Opcode command)
        {
            if (command == Opcode.DecreasePointer)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounters + ";" + Environment.NewLine})";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounter++;
            }
            else if (command == Opcode.IncreasePointer)
            {
                if (minusCounter > 0)
                {
                    output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                plusCounter++;
                // output +="++memory;\n";
            }
            else if (command == Opcode.IncreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"         ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                plusCounters++;
            }
            else if (command == Opcode.DecreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounters++;
            }
            else if (command == Opcode.Input)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"          cin>>*ptr;\n";
            }
            else if (command == Opcode.Output)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"          cout<<*ptr;\n";
            } else if (command == Opcode.Openloop) { 
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
            if (minusCounter > 0)
            {
                output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                minusCounter = 0;
            }
            if (minusCounters > 0)
            {
                output += $"          *ptr-={minusCounters + ";" + Environment.NewLine}";
                minusCounters = 0;
            }
            if (plusCounters > 0)
            {
                output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                plusCounters = 0;
            }
            output += $"             while(*ptr){{\n";
        }
            if (command == Opcode.Closeloop)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"         }}{Environment.NewLine}";
            }
            else if (command == Opcode.Result)
            {
                if (plusCounter > 0)
                {
                    output += $"          ptr+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          ptr-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          *ptr-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          *ptr+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
            }
        }
        public override void RunCode()
        {
            command = code;

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
                                if (loop == code.Length-3)
                                    Action(Opcode.Result);
                                //  output += "--memory;\n";
                                break;
                            case (char)Opcode.IncreasePointer://>
                                Action(Opcode.IncreasePointer);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.IncreaseDataPointer://+    
                                Action(Opcode.IncreaseDataPointer);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                //output +="ptr[memory]++;\n";
                                break;
                            case (char)Opcode.DecreaseDataPointer://-  
                                Action(Opcode.DecreaseDataPointer);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                // output +=" *ptr--;\n";
                                break;
                            case (char)Opcode.Output:
                                Action(Opcode.Output);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.Input:
                                Action(Opcode.Input);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.Openloop:
                                Action(Opcode.Openloop);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                break;
                            case (char)Opcode.Closeloop:
                                Action(Opcode.Closeloop);
                                if (loop == code.Length-3 )
                                    Action(Opcode.Result);
                                break;
                            case ' ':
                                Action(Opcode.Result);
                                return;
                        }
                        loop++;
                    }
                    catch (Exception E)
                    {
                        output = "Overflow Error!!";
                        return;
                    }
                }
                output = $@"#include<iostream>
using namespace std;
     int main(void)
        {{
         unsigned char * ptr=(unsigned char*)calloc('%d',1);
            {output}
        }}";
            }
        }
    }
}