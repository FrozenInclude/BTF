using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BTF
{
    public class JavaParser : InterPreter
    {
        private int ptrsize;
        private int plusCounter = 0;
        private int minusCounter = 0;
        private int plusCounters = 0;
        private int minusCounters = 0;
        private int loop { get; set; }
        private string command;
        public JavaParser(string code, int ptrsize) : base(code, ptrsize)
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
                    output += $"          memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          ptr[memory]-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounter++;
            }
            else if (command == Opcode.IncreasePointer)
            {
                if (minusCounter > 0)
                {
                    output += $"memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          ptr[memory]-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                plusCounter++;
            }
            else if (command == Opcode.IncreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output += $"          memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"         ptr[memory]-={minusCounter + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                plusCounters++;
            }
            else if (command == Opcode.DecreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output += $"         memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"         memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"         ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounters++;
            }
            else if (command == Opcode.Input)
            {
                if (plusCounter > 0)
                {
                    output += $"          memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          ptr[memory]-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $@"           try{{
      ptr[memory] = (char)System.in.read();
                        }} catch (IOException e)
                    {{
                        e.printStackTrace();
                    }}\n";
            }
            else if (command == Opcode.Output)
            {
                if (plusCounter > 0)
                {
                    output += $"        memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"        memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"         ptr[memory]-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"        ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"            System.out.println((char)ptr[memory]);\n";
            }
            else if (command == Opcode.Openloop)
            {
                if (plusCounter > 0)
                {
                    output += $"          memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"          memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"          ptr[memory]-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"              while(ptr[memory]!=0){{\n";
            }
            if (command == Opcode.Closeloop)
            {
                if (plusCounter > 0)
                {
                    output += $"         memory+={plusCounter + ";" + Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"            memory-={minusCounter + ";" + Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"           ptr[memory]-={minusCounters + ";" + Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"          ptr[memory]+={plusCounters + ";" + Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"          }}{Environment.NewLine}";
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
                output = $@"import java.util.*;
import java.lang.*;
import java.io.*;

class BTF
{{
	public static void main (String [] args) 
	{{
      char []ptr=new char[{ptrsize}];
      int memory=0;
      {output}
	}}
}}
";
            }
        }
    }
}