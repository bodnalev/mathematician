using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;

namespace Mathematician.NeuralNetwork
{
    /// <summary>
    /// Represents all the Neural Networks
    /// </summary>
    class Network
    {
        /// <summary>
        /// Stores the neuron numbers in the different layers including the input at [0] and the output at [Lenght-1]
        /// </summary>
        public int[] neuronNumbers;
        /// <summary>
        /// Stores the matrices for each layer
        /// </summary>
        public Matrix<float>[] matrices;
        /// <summary>
        /// Stores the biases for each layer
        /// </summary>
        public Vector<float>[] biases;

        /// <summary>
        /// True at [i] if the ith layer uses the lin transfer function
        /// </summary>
        public bool[] isLin;

        /// <summary>
        /// Network parameters for randomizing the values
        /// </summary>
        public double randomMin = -0.1, randomMax = 0.4;

        
        /// <summary>
        /// Constructs the network based on neuronNumbers,
        /// initializes the matrix and bias values with small random variables between randomMin and randomMax
        /// </summary>
        /// <param name="neuronNumbers">The array containing the neuron numbers in each layer (including the input)</param>
        /// <param name="isLin">The array storing wether each layer uses the linear transfer function</param>
        public Network(int[] neuronNumbers, bool[] isLin)
        {
            ContinuousUniform rand = new ContinuousUniform(randomMin, randomMax);
            this.neuronNumbers = neuronNumbers;
            matrices = new Matrix<float>[neuronNumbers.Length - 1];
            biases = new Vector<float>[neuronNumbers.Length - 1];
            this.isLin = new bool[neuronNumbers.Length - 1];
            for (int i = 0; i < isLin.Length && i < this.isLin.Length; i++)
            {
                this.isLin[i] = isLin[i];
            }
            for (int i = 0; i < neuronNumbers.Length - 1; i++)
            {
                matrices[i] = Matrix<float>.Build.Random(neuronNumbers[i + 1], neuronNumbers[i], rand);
                biases[i] = Vector<float>.Build.Random(neuronNumbers[i + 1], rand);
            }
        }

        /// <summary>
        /// Constructs the network based on neuronNumbers,
        /// initializes the matrix and bias values with small random variables between randomMin and randomMax
        /// </summary>
        /// <param name="neuronNumbers">The array containing the neuron numbers in each layer (including the input)</param>
        public Network(int[] neuronNumbers) : this(neuronNumbers, new bool[neuronNumbers.Length - 1])
        {
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
                input = isLin[i] ? (matrices[i] * input) + biases[i] : Functions.lsim((matrices[i] * input) + biases[i]);
            }
            midValues[M] = input.Clone();


            /*
             * Calculate the sensitivities backwards, using the midValues
             */
            Vector<float>[] sens = new Vector<float>[M];
            
            sens[M-1] = -2 * Functions.diffSimp(midValues[M]).PointwiseMultiply(output - midValues[M]);
            for (int i = M - 2; i >= 0; i--)
            {
                sens[i] = (isLin[i]?
                    //If the transfer function is linear then use the identity diagonal matrix
                    Matrix<float>.Build.DiagonalIdentity(midValues[i + 1].Count) :
                    //If the transfer function is log-sigmoid then use the diffSimp to calculate the derivative
                    Matrix<float>.Build.Diagonal((Functions.diffSimp(midValues[i + 1])).ToArray())) 
                    *(matrices[i + 1].Transpose()) * sens[i + 1];
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
                input = isLin[i] ? (matrices[i] * input) + biases[i] : Functions.lsim((matrices[i] * input)+biases[i]);
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

            Matrix<float> newMatrix;

            if (atLayer > 0)
            {
                Vector<float> newBias = Vector<float>.Build.Random(neuronNumbers[atLayer], rand);
                newBias.SetSubVector(0, biases[atLayer - 1].Count, biases[atLayer - 1]);
                biases[atLayer - 1] = newBias;

                newMatrix = Matrix<float>.Build.Random(neuronNumbers[atLayer - 1], neuronNumbers[atLayer], rand);
                newMatrix.SetSubMatrix(0, 0, matrices[atLayer - 1]);
                matrices[atLayer - 1] = newMatrix;
            }

            if (atLayer < matrices.Length)
            {
                newMatrix = Matrix<float>.Build.Random(neuronNumbers[atLayer], neuronNumbers[atLayer + 1], rand);
                newMatrix.SetSubMatrix(0, 0, matrices[atLayer]);
                matrices[atLayer] = newMatrix;
            }

        }

    }
}
