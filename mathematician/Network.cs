using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace mathematician
{
    class Network
    {
        public int[] layerMembers;
        Matrix<float>[] layers;
        Vector<float>[] biases;


        public int seed = 1337;

        public Network(int[] layerMembers)
        {
            Random rand = new Random();
            this.layerMembers = layerMembers;
            layers = new Matrix<float>[layerMembers.Length-1];
            biases = new Vector<float>[layerMembers.Length - 1];
            for (int i = 0; i < layerMembers.Length-1; i++)
            {
                layers[i] = Matrix<float>.Build.Random(layerMembers[i], layerMembers[i + 1], seed);
                biases[i] = Vector<float>.Build.Random(layerMembers[i+1],seed);
            }
        }

        public Vector<float> ResponseLsim(Vector<float> input)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                input = Functions.lsim((layers[i] * input)+biases[i]);
            }
            return input;
        }

        public Vector<float> ResponseStep(Vector<float> input)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                input = Functions.step((layers[i] * input) + biases[i]);
            }
            return input;
        }

    }
}
