using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

namespace NeuralNetwork
{
    class Network
    {
        /*
         *The variables storing the network current state
         */
        public int[] neuronNumbers;
        public Matrix<float>[] layers;
        public Vector<float>[] biases;

        //Network random parameters
        public double randomMin = -0.1, randomMax = 0.4;


        /// <summary>
        /// The network constructor, based on layerMembers,
        /// initializes the matrix and bias values with small random variables between randomMin and randomMax
        /// </summary>
        /// <param name="neuronNumbers">The array containing the neuron numbers in each layer (including the input)</param>
        public Network(int[] neuronNumbers)
        {
            ContinuousUniform rand = new ContinuousUniform(randomMin, randomMax);
            this.neuronNumbers = neuronNumbers;
            layers = new Matrix<float>[neuronNumbers.Length-1];
            biases = new Vector<float>[neuronNumbers.Length - 1];
            for (int i = 0; i < neuronNumbers.Length-1; i++)
            {
                layers[i] = Matrix<float>.Build.Random(neuronNumbers[i + 1], neuronNumbers[i], rand);
                biases[i] = Vector<float>.Build.Random(neuronNumbers[i + 1], rand);
            }
        }

        /// <summary>
        /// Trains the network with the given input/output pairs
        /// </summary>
        /// <param name="input">The input vector</param>
        /// <param name="output">The expected output vector</param>
        /// <param name="alpha">The training parameter</param>
        public void Train(Vector<float> input, Vector<float> output, float alpha)
        {
            int M = layers.Length;

            /*
             * Calculate the mid values, the response of each layer between the first and last
             * to use this in the backpropagation
             */
            Vector<float>[] midValues = new Vector<float>[M+1];
            
            for (int i = 0; i < M; i++)
            {
                midValues[i] = input.Clone();
                input = Functions.lsim((layers[i] * input) + biases[i]);
            }
            midValues[M] = input.Clone();


            /*
             * Calculate the sensitivities backwards, using the midValues
             */
            Vector<float>[] sens = new Vector<float>[M];
            
            sens[M-1] = -2 * Functions.diffSimp(midValues[M]).PointwiseMultiply(output - midValues[M]);
            for (int i = M - 2; i >= 0; i--)
            {
                sens[i] = Matrix<float>.Build.Diagonal((Functions.diffSimp(midValues[i+1])).ToArray()) * (layers[i + 1].Transpose()) * sens[i + 1];
            }

            /*
             * Modify the matrix and bias elements
             */

            for (int i = 0; i < M; i++)
            {
                layers[i] = layers[i] - alpha * sens[i].OuterProduct(midValues[i]);
                biases[i] = biases[i] - alpha * sens[i];
            }

        }
        
        
        /// <summary>
        /// Calculates the response of the network for a given input
        /// </summary>
        /// <param name="input">The given input</param>
        /// <returns></returns>
        public Vector<float> Response(Vector<float> input)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                input = Functions.lsim((layers[i] * input)+biases[i]);
            }
            return input;
        }

    }
}
