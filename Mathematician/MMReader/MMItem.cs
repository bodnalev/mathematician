using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematician.MMReader
{
    class MMItem
    {
        public enum MMItemType {Special, Name, Symbol, Wff}
        protected MMItemType type;
    }
}
