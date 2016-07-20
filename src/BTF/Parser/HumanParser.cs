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
        private CellSize cell = CellSize.bit8;
        private int loop { get; set; }
        private bool memoryView;
        private bool syntaxCheck = false;
        private int TimeoutCount = 0;
        private string traceLog;
        private int memory=0;
        private string command;
        private bool? DisplayIns = false;
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
        public HumanParser(string code, CellSize cell,bool? Displayins,int ptrsize=0,bool memoryview=false,bool checkOnlySyntax=false) : base(code, ptrsize)
        {
            this.ptrsize = ptrsize;
            this.cell = cell;
            this.DisplayIns = Displayins;
            this.memoryView = memoryview;
            this.syntaxCheck = checkOnlySyntax;
            if (this.ptrsize != 0)
            {
                if (this.cell == CellSize.bit8)
                {
                    ptr = new List<byte>(ptrsize){0 };
                }
                else if (this.cell == CellSize.bit16)
                {
                    ptr16 = new List<ushort>(ptrsize) { 0 };
                }
                else if (this.cell == CellSize.bit32)
                {
                    ptr32 = new List<uint>(ptrsize) { 0 };
                }
            }
        }
     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Action(Opcode command)
        {
            try {
                if (cell == CellSize.bit8)
                {
                    if (command == Opcode.DecreasePointer)
                    {
                        --memory;
                    }
                    else if (command == Opcode.IncreasePointer)
                    {
                        ++memory;
                        //  ptr.Add(0);
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
                        catch (Exception)
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
                            traceLog += $"(pointer.{memory}  {ptr[memory]}  {(char)ptr[memory]}) 지점 '{(char)ptr[memory]}' 출력\n";
                        }
                    }
                } else if (cell == CellSize.bit16)
                {
                    if (command == Opcode.DecreasePointer)
                    {
                        --memory;
                    }
                    else if (command == Opcode.IncreasePointer)
                    {
                        ++memory;
                        ptr16.Add(0);
                    }
                    else if (command == Opcode.IncreaseDataPointer)
                    {
                        if (memory > -1)
                        {
                            ++ptr16[memory];
                        }
                    }
                    else if (command == Opcode.DecreaseDataPointer)
                    {
                        if (memory > -1)
                        {
                            --ptr16[memory];
                        }
                    }
                    else if (command == Opcode.Input)
                    {
                        try
                        {
                            char[] part = Microsoft.VisualBasic.Interaction.InputBox(@"INPUT(첫번째 첫문자로 짤립니다.) 입력종료는 '0'을써주세요", "포인터 입력", "").ToCharArray();
                            if (part[0] == '0')
                            {
                                ptr16[memory] = 0;
                            }
                            else
                            {
                                ptr16[memory] = (ushort)part[0];
                            }

                        }
                        catch (Exception)
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
                            char st = (char)ptr16.ElementAt(memory);
                            output += st.ToString();
                            traceLog += $"(pointer.{memory}  {ptr16[memory]}  {(char)ptr16[memory]}) 지점 '{(char)ptr16[memory]}' 출력\n";
                        }
                    }
                }
                else if (cell == CellSize.bit32)
                {
                    if (command == Opcode.DecreasePointer)
                    {
                        --memory;
                    }
                    else if (command == Opcode.IncreasePointer)
                    {
                        ++memory;
                        ptr32.Add(0);
                    }
                    else if (command == Opcode.IncreaseDataPointer)
                    {
                        if (memory > -1)
                        {
                            ++ptr32[memory];
                        }
                    }
                    else if (command == Opcode.DecreaseDataPointer)
                    {
                        if (memory > -1)
                        {
                            --ptr32[memory];
                        }
                    }
                    else if (command == Opcode.Input)
                    {
                        try
                        {
                            char[] part = Microsoft.VisualBasic.Interaction.InputBox(@"INPUT(첫번째 첫문자로 짤립니다.) 입력종료는 '0'을써주세요", "포인터 입력", "").ToCharArray();
                            if (part[0] == '0')
                            {
                                ptr32[memory] = 0;
                            }
                            else
                            {
                                ptr32[memory] = (uint)part[0];
                            }

                        }
                        catch (Exception)
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
                            char st = (char)ptr32.ElementAt(memory);
                            output += st.ToString();
                            traceLog += $"(pointer.{memory}  {ptr32[memory]}  {(char)ptr32[memory]}) 지점 '{(char)ptr32[memory]}' 출력\n";
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                if(cell==CellSize.bit8)
                ptr.Add(0);
                else if (cell == CellSize.bit16)
                    ptr16.Add(0);
                else if (cell == CellSize.bit32)
                    ptr32.Add(0);
                loop = loop - 1;
            }
        }
        public override void RunCode()
        {
            command = code;
            if (code != null)
            {
                while (loop < code.Length)
                {
                    if (pause == true)
                    {
                        output = "번역이 중단되었습니다.";
                        return;
                    }
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
                                if (cell == CellSize.bit8)
                                {
                                    if (syntaxCheck)
                                    {
                                        TimeoutCount += 1;
                                        if (TimeoutCount == 100)
                                        {
                                            return;
                                        }
                                    }
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
                                }
                                else if (cell == CellSize.bit16)
                                {
                                    if (syntaxCheck)
                                    {
                                        TimeoutCount += 1;
                                        if (TimeoutCount == 100)
                                        {
                                            return;
                                        }
                                    }
                                    if (ptr16[memory] == 0)
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
                                }
                                else if (cell == CellSize.bit32)
                                {
                                    if (syntaxCheck)
                                    {
                                        TimeoutCount += 1;
                                        if (TimeoutCount == 100)
                                        {
                                            return;
                                        }
                                    }
                                    if (ptr32[memory] == 0)
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
                                }
                                break;
                            case (char)Opcode.Closeloop:
                                if (cell == CellSize.bit8)
                                {
                                    if (syntaxCheck)
                                    {
                                        TimeoutCount += 1;
                                        if (TimeoutCount == 100)
                                        {
                                            return;
                                        }
                                    }
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
                                }
                                else if (cell == CellSize.bit16)
                                {
                                    if (syntaxCheck)
                                    {
                                        TimeoutCount += 1;
                                        if (TimeoutCount == 100)
                                        {
                                            return;
                                        }
                                    }
                                    if (ptr16[memory] != 0)
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
                                }
                               else if (cell == CellSize.bit32)
                                {
                                    if (syntaxCheck)
                                    {
                                        TimeoutCount += 1;
                                        if (TimeoutCount == 100)
                                        {
                                            return;
                                        }
                                    }
                                    if (ptr32[memory] != 0)
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
                if(DisplayIns==true)
                output += $"\n\n\nInstruction pointer:{loop-2}";
                if (memoryView)
                {
                    output += $"\n\n\n메모리 뷰\n\n";
                    if (cell == CellSize.bit8)
                    {
                        for (int i = 0; i < ptr.Count; i++)
                        {
                            output += $"pointer.{i}  {ptr[i]}  {(char)ptr[i]}\n";
                        }
                    }
                    else if (cell == CellSize.bit16)
                    {
                        for (int i = 0; i < ptr16.Count; i++)
                        {
                            output += $"pointer.{i}  {ptr16[i]}  {(char)ptr16[i]}\n";
                        }
                    }
                    else if (cell == CellSize.bit32)
                    {
                        for (int i = 0; i < ptr32.Count; i++)
                        {
                            output += $"pointer.{i}  {ptr32[i]}  {(char)ptr32[i]}\n";
                        }
                    }
                    output += $"\n\n\n출력로그\n\n{traceLog}";
                }
            }
        }
    }
}
