﻿using System;
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
                                break;
                        }
                        loop++;
                    }
                    catch (Exception)
                    {
                        output = "Overflow Error!!";
                        return;
                    }
                }
                output = $@"{ output}";
            }
        }
    }
}
