using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace mathematician
{
    class Functions
    {
        public static float step(float f)
        {
            if (f > 0)
            {
                return 1;
            }
            return 0;
        }

        public static float lsim(float f)
        {
            return 1 / (1 + (float)Math.Exp(-f));
        }

        public static Vector<float> step(Vector<float> v)
        {
            float[] ar = v.AsArray();
            for (int i = 0; i < ar.Length; i++)
            {
                ar[i] = ar[i] > 0 ? 1 : 0;
            }
            return Vector<float>.Build.Dense(ar);
        }

        public static Vector<float> lsim(Vector<float> v)
        {
            return (Vector<float>.Exp(-v) + Vector<float>.Build.Dense(v.Count, 1)).DivideByThis(1);
        }

    }
}
