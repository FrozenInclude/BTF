using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BTF
{
    public class SchemeParser : InterPreter
    {
        private int ptrsize;
        private string 들여쓰기 = "";
        private int nested = 0;
        private string last들여쓰기 = "";
        private int plusCounter = 0;
        private int minusCounter = 0;
        private int plusCounters = 0;
        private int minusCounters = 0;
        private int loop { get; set; }
        private string command;
        public SchemeParser(string code, int ptrsize) : base(code, ptrsize)
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
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounter++;
            }
            else if (command == Opcode.IncreasePointer)
            {
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                plusCounter++;
                // output +="++memory;\n";
            }
            else if (command == Opcode.IncreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                plusCounters++;
            }
            else if (command == Opcode.DecreaseDataPointer)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                minusCounters++;
            }
            else if (command == Opcode.Input)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                output += "ptr[memory]=readLine(stripNewline: true)\n";
            }
            else if (command == Opcode.Output)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $"{들여쓰기}(display (integer->char (vector-ref ptr memory)))\n";
            }
            else if (command == Opcode.Openloop)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                    output += $@"(while (not (eq? (vector-ref ptr memory) 0)){Environment.NewLine}";
            }
            if (command == Opcode.Closeloop)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
                output += $@"){Environment.NewLine}";
            }
            else if (command == Opcode.Result)
            {
                if (plusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (+ memory {plusCounter})){Environment.NewLine}";
                    plusCounter = 0;
                }
                if (minusCounter > 0)
                {
                    output += $"{들여쓰기}(set! memory (- memory {minusCounter})){Environment.NewLine}";
                    minusCounter = 0;
                }
                if (minusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (- {minusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    minusCounters = 0;
                }
                if (plusCounters > 0)
                {
                    output += $"{들여쓰기}(vector-set! ptr memory (+ {plusCounters} (vector-ref ptr memory))){Environment.NewLine}";
                    plusCounters = 0;
                }
            }
        }

        public override void RunCode()
        {
            command = code;
            nested = 0;
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
                                return;
                        }
                        loop++;
                    }
                    catch (Exception)
                    {
                        output = "Overflow Error!!";
                        return;
                    }
                }
                output = $@"(define ptr (make-vector {ptrsize} 0))
(define memory 0)
(define-syntax loop
	(ir-macro-transformer
		(lambda (expr inject compare)
			(let ((body (cdr expr)))
				`(call-with-current-continuation
					(lambda (,(inject 'exit))
						(let f () ,@body (f))))))))

(define-syntax while
	(ir-macro-transformer
		(lambda (expr inject compare)
			(let ((test (cadr expr))
				    (body (cddr expr)))
				`(loop
				(if (not ,test) (,(inject 'exit) #f))
				,@body)))))
{ output}";
            }
        }
    }
}