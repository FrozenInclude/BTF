using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BTF
{
    public class OokParser : InterPreter
    {
        private int loop;
        private string command;
        public OokParser(string code, int ptrsize) : base(code, ptrsize)
        {
         
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Action(Opcode command)
        {
            if (command == Opcode.DecreasePointer)
            {
                output += "Ook? Ook.";
            }
            else if (command == Opcode.IncreasePointer)
            {
                output += "Ook. Ook?";
            }
            else if (command == Opcode.IncreaseDataPointer)
            {
                output += "Ook.Ook.";
            }
            else if (command == Opcode.DecreaseDataPointer)
            {
                output += "Ook! Ook!";
            }
            else if (command == Opcode.Input)
            {
                output += "Ook. Ook!";
            }
            else if (command == Opcode.Output)
            {
                output += "Ook! Ook.";
            }
            else if (command == Opcode.Openloop)
            {
                output += "Ook! Ook?";
            }
            if (command == Opcode.Closeloop)
            {
          output += "Ook? Ook!";
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
                                //  output += "--memory;\n";
                                break;
                            case (char)Opcode.IncreasePointer://>
                                Action(Opcode.IncreasePointer);
                                break;
                            case (char)Opcode.IncreaseDataPointer://+    
                                Action(Opcode.IncreaseDataPointer);
                                //output +="*ptr++;\n";
                                break;
                            case (char)Opcode.DecreaseDataPointer://-  
                                Action(Opcode.DecreaseDataPointer);
                                // output +=" *ptr--;\n";
                                break;
                            case (char)Opcode.Output:
                                Action(Opcode.Output);
                                break;
                            case (char)Opcode.Input:
                                Action(Opcode.Input);
                                break;
                            case (char)Opcode.Openloop:
                                Action(Opcode.Openloop);
                                break;
                            case (char)Opcode.Closeloop:
                                Action(Opcode.Closeloop);
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
