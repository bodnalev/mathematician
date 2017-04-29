using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

namespace Mathematician.NeuralNetwork
{
    class Network
    {
        /*
         *The variables storing the network current state
         */
        public int[] neuronNumbers;
        public Matrix<float>[] matrices;
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
            matrices = new Matrix<float>[neuronNumbers.Length-1];
            biases = new Vector<float>[neuronNumbers.Length - 1];
            for (int i = 0; i < neuronNumbers.Length-1; i++)
            {
                matrices[i] = Matrix<float>.Build.Random(neuronNumbers[i + 1], neuronNumbers[i], rand);
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
            int M = matrices.Length;

            /*
             * Calculate the mid values, the response of each layer between the first and last
             * to use this in the backpropagation
             */
            Vector<float>[] midValues = new Vector<float>[M+1];
            
            for (int i = 0; i < M; i++)
            {
                midValues[i] = input.Clone();
                input = Functions.lsim((matrices[i] * input) + biases[i]);
            }
            midValues[M] = input.Clone();


            /*
             * Calculate the sensitivities backwards, using the midValues
             */
            Vector<float>[] sens = new Vector<float>[M];
            
            sens[M-1] = -2 * Functions.diffSimp(midValues[M]).PointwiseMultiply(output - midValues[M]);
            for (int i = M - 2; i >= 0; i--)
            {
                sens[i] = Matrix<float>.Build.Diagonal((Functions.diffSimp(midValues[i+1])).ToArray()) * (matrices[i + 1].Transpose()) * sens[i + 1];
            }

            /*
             * Modify the matrix and bias elements
             */

            for (int i = 0; i < M; i++)
            {
                matrices[i] = matrices[i] - alpha * sens[i].OuterProduct(midValues[i]);
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
            for (int i = 0; i < matrices.Length; i++)
            {
                input = Functions.lsim((matrices[i] * input)+biases[i]);
            }
            return input;
        }

        /// <summary>
        /// Extends the network's neuron numbers at a given layer.
        /// </summary>
        /// <param name="extra">The extra neurons</param>
        /// <param name="atLayer">The layer to extend</param>
        /// <param name="defValue">The default value to fill</param>
        public void ExtendAt(int extra, int atLayer, float defValue)
        {
            ContinuousUniform rand = new ContinuousUniform(randomMin, randomMax);
            neuronNumbers[atLayer] += extra;

            Matrix<float> newMatrix = Matrix<float>.Build.Dense(neuronNumbers[atLayer], neuronNumbers[atLayer + 1], (float)rand.Sample());
            newMatrix.SetSubMatrix(0, 0, matrices[atLayer - 1]);

            if (atLayer > 0)
            {
                Vector<float> newBias = Vector<float>.Build.Dense(neuronNumbers[atLayer], (float)rand.Sample());
                newBias.SetSubVector(0, biases[atLayer - 1].Count, biases[atLayer - 1]);
                biases[atLayer - 1] = newBias;

                newMatrix = Matrix<float>.Build.Dense(neuronNumbers[atLayer - 1], neuronNumbers[atLayer]);
                newMatrix.SetSubMatrix(0, 0, matrices[atLayer - 1]);
                
            }

            
        }

    }
}
