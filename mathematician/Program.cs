using System;
using Mathematician.NeuralNetwork;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;
using System.Diagnostics; //for Stopwatch
using Mathematician.MMVerifier;

namespace Mathematician
{
    class Program
    {
        static void Main(string[] args)
        {
            //MMFile.FromFile("D:\\Edu\\Programming\\Mathematician\\Mathematician\\set.txt");
            MMProver prover = new MMProver("D:\\Edu\\Programming\\Mathematician\\Mathematician\\set.txt");
            prover.Read();

            Console.Read();
        }

        
    }
}
