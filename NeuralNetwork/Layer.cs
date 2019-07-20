using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Layer
    {
        public List<Neuron> Neurons { get; }

        public Layer()
        {
            Neurons = new List<Neuron>();
        }

        public void AddNeuron(Neuron neuron) => Neurons.Add(neuron);
    }
}
