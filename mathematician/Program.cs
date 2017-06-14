using System;
using Mathematician.NeuralNetwork;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;
using System.Diagnostics;
using Mathematician.MMVerifier;

namespace Mathematician
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                MMFile.FromFile("D:\\Edu\\Programming\\Mathematician\\Mathematician\\set.txt");
            }
            catch (Exception e) { };
            sw.Stop();
            Console.WriteLine("delta time: "+sw.Elapsed);
            Console.Read();
        }

        static void MinimizeError(Network n, float limit, int inLimit)
        {
            ContinuousUniform rand = new ContinuousUniform(-2, 2);
            Vector<float> input, output;
            float original, network, error;
            int maxIterations = 100000, currIterations = 1;
            int currInsideLimit = 0;
            while (currIterations < maxIterations && currInsideLimit < inLimit)
            {
                
                //Train
                float f = (float)rand.Sample();
                input = NumToVec(f);
                output = NumToVec(FuncApprox(f));
                n.Train(input, output, 0.005f+1f/(currIterations));

                //Test
                f = (float)rand.Sample();
                input = NumToVec(f);
                output = n.Response(input);
                original = FuncApprox(f);
                network = VecToNum(output);


                error = Math.Abs(original - network);
                Console.WriteLine("Error: " + error);
                if (error < limit)
                {
                    currInsideLimit++;
                }
                else
                {
                    currInsideLimit = 0;
                }
                currIterations++;
            }
            if (currIterations == maxIterations)
            {
                Console.WriteLine("Iterations ran out");
            }
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
                n.Train(input, output, 0.1f);
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
                Console.WriteLine("Error: " + (original - network) + "  \t Original: " + original + "\t Network: " + network);
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
            return (1 + (float) Math.Sin(input * Math.PI / 8))/3;
        }
    }
}
