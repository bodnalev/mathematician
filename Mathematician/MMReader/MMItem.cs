using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematician.MMReader
{
    class MMItem
    {
        public enum MMItemType {Syntax, Name, Constant, Variable, Null}
        public MMItemType type;
        public string content;

        public static List<string> constants;
        public static List<string> variables;

        public MMItem(string content)
        {
            if (constants == null)
            {
                constants = new List<string>();
            }
            if (variables == null)
            {
                variables = new List<string>();
            }

            this.content = content;
            type = FindType();
        }

        private MMItemType FindType()
        {
            char[] a = content.ToCharArray();
            int l = a.Length;
            if (l == 0)
            {
                return MMItemType.Null;
            }

            if (a[0] == '$')
            {
                return MMItemType.Syntax;
            }

            foreach (string s in constants)
            {
                if (s.Equals(a))
                {
                    return MMItemType.Constant;
                }
            }
            foreach (string s in variables)
            {
                if (s.Equals(a))
                {
                    return MMItemType.Variable;
                }
            }
            return MMItemType.Name;
        }

    }
}
