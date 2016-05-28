using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows;

namespace BTF
{
    public abstract class InterPreter
    {
        protected byte[] ptr;
        public string code { get; set; }
        private int size;
        public bool error { get; set; } = false;
        public string output { get; set; }
        public InterPreter(string code, int ptrsize)
        {
            this.code = code;
            this.size = ptrsize;
            ptr = new byte[size];
        }
        public abstract void RunCode();
        public int LoopD(string str, int start)
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
        }
        public int LoopS(string str, int start)
        {
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
    }
}
