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
        static List<string> tags = new List<string>();
        static List<char> specials = new List<char>();
        #region ctor
        static Highlighter()
        {
            string[] strs = {
                "Anchor",
                @".",
                @"[",
                @"]",
                @",",
                
             };

            tags = new List<string>(strs);
            char[] chrs = {
                '<',
                '>',
                '+',
                '-',
                ';',
                '\n',
                '\t'
            };
            specials = new List<char>(chrs);
        }

        #endregion
        public static List<char> GetSpecials
        {
            get { return specials; }
        }
        public static List<string> GetTags
        {
            get { return tags; }
        }
        public static bool IsKnownTag(string tag)
        {
            return tags.Exists(delegate (string s) { return s.ToLower().Equals(tag.ToLower()); });
        }
        public static List<string> GetJSProvider(string tag)
        {
            return tags.FindAll(delegate (string s) { return s.ToLower().StartsWith(tag.ToLower()); });
        }
    }
}
      