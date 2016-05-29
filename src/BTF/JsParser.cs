using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTF
{
    public class JsParser : InterPreter
    {
        private int ptrsize;
        private int plusCounter=0;
        private int minusCounter = 0;
        private int loop { get; set; }
        private string command;
        public JsParser(string code, int ptrsize) : base(code, ptrsize)
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
                                output += "--memory;\n";
                                break;
                            case '>'://>
                                output +="++memory;\n";
                                break;
                            case '+'://+
                                output +="ptr[memory]++;\n";
                                break;
                            case '-'://-
                               output+=" ptr[memory]--;\n";
                                break;
                            case '.':
                                output +=$"console.log(String.fromCharCode(ptr[memory]));\n";
                                break;
                            case '[':
                                output += $"while(ptr[memory]){{\n";
                                break;
                            case ']':
                                output += $"}}{Environment.NewLine}";
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
                output = $@"var ptr=new Array();!var memory=0;!for(var i=0;i<{ptrsize};i++){{!ptr[i]=0;!}}!{output}";
                output=output.Replace("!", Environment.NewLine);
            }
        }
    }
    }


