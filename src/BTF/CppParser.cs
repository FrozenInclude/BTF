using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                //  output += "--memory;\n";
                                break;
                            case (char)Opcode.IncreasePointer://>
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
                                break;
                            case (char)Opcode.IncreaseDataPointer://+    
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
                                //output +="*ptr++;\n";
                                break;
                            case (char)Opcode.DecreaseDataPointer://-  
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
                                // output +=" *ptr--;\n";
                                break;
                            case (char)Opcode.Output:
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
                                break;
                            case (char)Opcode.Input:
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
                                break;
                            case (char)Opcode.Openloop:
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
                                break;
                            case (char)Opcode.Closeloop:
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