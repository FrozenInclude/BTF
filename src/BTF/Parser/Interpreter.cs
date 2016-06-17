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
        protected List<byte> ptr=new List<byte> {0};
        public enum Opcode{IncreasePointer='>',DecreasePointer='<',IncreaseDataPointer='+',DecreaseDataPointer='-',Output='.',Input=',',Openloop='[',Closeloop=']' }
        public string code { get; set; }
        private int size;
        public bool error { get; set; } = false;
        public string output { get; set; }
        public InterPreter(string code, int ptrsize)
        {
            this.code = code;
            this.size = ptrsize;
        //    ptr = new byte[size];
        }
        public abstract void RunCode();
        protected abstract void Action(Opcode command);
    }
}
