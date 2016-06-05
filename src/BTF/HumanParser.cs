using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                            case '<':
                                --memory;
                                break;
                            case '>'://>
                                ++memory;
                                ptr.Add(0);
                                break;
                            case '+'://+
                                if (memory >-1)
                                {
                                    ++ptr[memory];
                                }
                                break;
                            case '-'://-
                                if (memory >-1)
                                {
                                    --ptr[memory];
                                }
                                break;
                            case '.':
                                if (memory >=0)
                                {
                                    char st = (char)ptr[memory];
                                    output += st.ToString();
                                }
                                break;
                            case ',':
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
                                break;
                            case '[':
                                if (ptr[memory] == 0)
                                {
                                    var backloop = loop;
                                    loop = Loop(command, loop);
                                    if (loop == -1)
                                    {
                                        output = $"{backloop + 1}번째  문법오류:']'가필요합니다.";
                                        error = true;
                                        return;
                                    }
                                }
                                break;
                            case ']':
                                if (ptr[memory] != 0)
                                {
                                    var backloop = loop;
                                    loop = Loop(command, loop,false);
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
