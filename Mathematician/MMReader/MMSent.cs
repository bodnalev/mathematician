using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematician.MMReader
{
    class MMSent
    {
        protected MMItem name;
        protected List<MMItem> content;
        public enum MMSentType {ConstantDec, VariableDec, DisjointVarRest,
            FloatingHyp, EssentialHyp, Axiom, TheoremProof}
        protected MMSentType type;

    }
}
