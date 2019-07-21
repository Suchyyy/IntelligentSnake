using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }

        public Layer()
        {
            Neurons = new List<Neuron>();
        }

        public void AddNeuron(Neuron neuron) => Neurons.Add(neuron);

        public void CalculateOutput()
        {
            foreach (var n in Neurons)
            {
                var t = n.Output;
            }
        }
    }
}
