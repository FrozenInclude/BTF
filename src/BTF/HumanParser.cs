using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BTF
{
    public class HumanParser : InterPreter
    {
        private int ptrsize;
        private int loop { get; set; }
        private int memory=0;
        private string command;
        public int Loop(string str, int start, bool boo=true)
        {
            if (boo == true)
            {
                int c = 0;
                for (int i = start + 1; i < str.Length; ++i)
                {
                    if (str[i] == '[')
                    {
                        c++;
                    }
                    if (str[i] == ']')
                    {
                        if (c == 0)
                        {
                            return i;
                        }
                        else
                        {
                            c--;
                        }
                    }
                }
                return -1;
            }else if (boo == false) {
                int c = 0;
                string rev = new string(str.Reverse().ToArray());
                int rev_start = str.Length - start - 1;
                for (int i = rev_start + 1; i < rev.Length; ++i)
                {
                    if (rev[i] == ']')
                    {
                        c++;
                    }
                    if (rev[i] == '[')
                    {
                        if (c == 0)
                        {
                            return str.Length - i - 1;
                        }
                        else
                        {
                            c--;
                        }
                    }
                }
                return -1;
            }
            return 0;
        }
        public HumanParser(string code, int ptrsize) : base(code, ptrsize)
        {
            this.ptrsize = ptrsize;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Action(Opcode command)
        {
            if (command == Opcode.DecreasePointer)
            {
                --memory;
            }
            else if (command == Opcode.IncreasePointer)
            {
                ++memory;
                ptr.Add(0);
            }
            else if (command == Opcode.IncreaseDataPointer)
            {
                if (memory > -1)
                {
                    ++ptr[memory];
                }
            }
            else if (command == Opcode.DecreaseDataPointer)
            {
                if (memory > -1)
                {
                    --ptr[memory];
                }
            }
            else if (command == Opcode.Input)
            {
                try
                {
                    char[] part = Microsoft.VisualBasic.Interaction.InputBox(@"INPUT(첫번째 첫문자로 짤립니다.) 입력종료는 '0'을써주세요", "포인터 입력", "").ToCharArray();
                    if (part[0] == '0')
                    {
                        ptr[memory] = 0;
                    }
                    else
                    {
                        ptr[memory] = (byte)part[0];
                    }

                }
                catch (Exception E)
                {
                    var backloop = loop;
                    output = $"{backloop + 1}번째  문법오류:잘못된 입력입니다.";
                    return;
                }
            }
            else if (command == Opcode.Output)
            {
                if (memory >= 0)
                {
                    char st = (char)ptr.ElementAt(memory);
                    output += st.ToString();
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
                                break;
                            case (char)Opcode.IncreasePointer://>
                                Action(Opcode.IncreasePointer);
                                break;
                            case (char)Opcode.IncreaseDataPointer://+
                                Action(Opcode.IncreaseDataPointer);
                                break;
                            case (char)Opcode.DecreaseDataPointer://-
                                Action(Opcode.DecreaseDataPointer);
                                break;
                            case (char)Opcode.Output:
                                Action(Opcode.Output);
                                break;
                            case (char)Opcode.Input:
                                Action(Opcode.Input);
                                break;
                            case (char)Opcode.Openloop://반복문은 따로생각.
                                if (ptr[memory] == 0)
                                {
                                    var backloop = loop;
                                    loop = Loop(code, loop);
                                    if (loop == -1)
                                    {
                                        output = $"{backloop + 1}번째  문법오류:']'가필요합니다.";
                                        error = true;
                                        return;
                                    }
                                }
                                break;
                            case (char)Opcode.Closeloop:
                                if (ptr[memory] != 0)
                                {
                                    var backloop = loop;
                                    loop = Loop(code, loop, false);
                                    if (loop == -1)
                                    {
                                        output = $"{backloop}번째  문법오류:'['가필요합니다.";
                                        error = true;
                                        return;
                                    }
                                }
                                break;
                        }
                        loop++;
                    }
                    catch (Exception E)
                    {
                        output = "Overflow Error!!"+E.ToString()+memory;
                        return;
                    }
                }
                output += $"\n\n\nInstruction pointer:{loop}";
            }
        }
    }
}
