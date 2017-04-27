using System;
using NeuralNetwork;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

namespace Mathematician
{
    class Program
    {
        static void Main(string[] args)
        {
            Network n = new Network(new int[] {1, 2, 1});
            Train(1000, n);
            Test(10, n);
            Console.ReadLine();
        }



        static void Train(int iteration, Network n)
        {
            ContinuousUniform rand = new ContinuousUniform(-2, 2);
            Vector<float> input, output;
            for (int i = 0; i < iteration; i++)
            {
                float f =(float) rand.Sample();
                input = NumToVec(f);
                output = NumToVec(FuncApprox(f));
                n.Train(input, output, 0.2f);
            }
        }

        static void Test(int iteration, Network n)
        {
            ContinuousUniform rand = new ContinuousUniform(-2, 2);
            Vector<float> input, output;
            float original, network;
            for (int i = 0; i < iteration; i++)
            {
                float f = (float)rand.Sample();
                input = NumToVec(f);
                output = n.Response(input);
                original = FuncApprox(f);
                network = VecToNum(output);
                Console.WriteLine("Error: " + (original - network) + "\t Original: " + original + "\t Network: " + network);
            }
        }

        static Vector<float> NumToVec(float f)
        {
            return Vector<float>.Build.Dense(1, f);
        }

        static float VecToNum(Vector<float> v)
        {
            return v.At(0);
        }

        static float FuncApprox(float input)
        {
            return 1 + (float) Math.Sin(input * Math.PI / 4);
        }
    }
}
