using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTF
{
    public class OokParser : InterPreter
    {
        private int loop;
        private char[] command;
        public OokParser(string code, int ptrsize) : base(code, ptrsize)
        {
            this.command = code.ToCharArray();
        }
        public override void RunCode()
        {
            if (this.command != null)
            {
                while (loop < command.Length)
                {
                    switch (command[loop])
                    {
                        case (char)Opcode.DecreasePointer:
                            output += "Ook? Ook.";
                            break;
                        case (char)Opcode.IncreasePointer://>
                            output += "Ook. Ook?";
                            break;
                        case (char)Opcode.IncreaseDataPointer://+
                            output += "Ook.Ook.";
                            break;
                        case (char)Opcode.DecreaseDataPointer://-
                            output += "Ook! Ook!";
                            break;
                        case (char)Opcode.Output:
                            // Console.Write(ptr[memory]);
                            output += "Ook! Ook.";
                            break;

                        case (char)Opcode.Input:
                            output += "Ook. Ook!";
                            break;
                        case (char)Opcode.Openloop:
                            output += "Ook! Ook?";
                            break;
                        case (char)Opcode.Closeloop:
                            output += "Ook? Ook!";
                            break;
                    }

                    loop++;
                }
            }
        }
    }
}
