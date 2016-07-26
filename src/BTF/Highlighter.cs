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
namespace highlight
{
    class Highlighter
    {
        static List<string> tag = new List<string>();
        static List<char> specials = new List<char>();
        static List<char> special = new List<char>();
        #region ctor
        static Highlighter()
        {
            string[] str = {
                "Anchor",@"""fmt""",
                    "console",
                        "log",
                            "for",
                                "while",
                                 "Array"
                                 ,"new","@\"%s\"",@"""%c""",
                "String","fromCharCode","using","Write","Read","List","static","void","byte","public","class","int","char","Add","namespace","unsigned",@"#include","import","String","'%d'","byte","chr","print","Dim","New","Integer","iostream","\"%d\"",
                "AS","Imports","Module","Sub","List","While","Byte","Add","Char","End","Of","CByte","fn","my","$","echo","Program","type","begin","catch","try","IOException","true","return","drain","NSLog","#import","NSAutoreleasePool","alloc","calloc",
                "var",
                "trace","fmt","func","package","mutable","let","open","do","end","use","std","mut","u8","i32","match",@"""""",@"""{}""","\"{:?}\"","define","define-syntax","lambda","vector-set!","set!","display","call-with-current-continuation","make-vector","vector-ref","Foundation/Foundation.h"
             };
            tag = new List<string>(str);
            char[] chrs = {
  
                '.',
                ' ',
                ',',
                '<',
                '>',
                '+',
                '-',
                '\n',
                '\t'
                ,

            };
            char[] chr = {
                '[',
                ']',
                '(',
                '=',
                ')',
                '.',
                '<',
                '>',
                '*',
                ',',
              //  '{',
                ':'
            };
            specials = new List<char>(chrs);
            special = new List<char>(chr);
        }

        #endregion
        public static List<char> GetSpecials
        {
            get { return specials; }
        }
        public static List<char> GetSpecial
        {
            get { return special; }
        }
    
        public static bool IsKnown(string tag)
        {
            return Highlighter.tag.Exists(delegate (string s) { return s.ToLower().Equals(tag.ToLower()); });
        }
        
    }
}
