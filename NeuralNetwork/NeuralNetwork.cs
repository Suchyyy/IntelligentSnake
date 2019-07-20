using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        public List<Input> Inputs { get; }
        public List<Layer> Layers { get; }

        public NeuralNetwork(int inputCount)
        {
            Layers = new List<Layer>();
            Inputs = new List<Input>();

            for (var i = 0; i < inputCount; i++) Inputs.Add(new Input(0.0));
        }

        public void AddLayer(int size)
        {
            if (Layers.Count == 0) AddInputLayer(size);
            else AddInnerLayer(size);
        }

        private void AddInputLayer(int size)
        {
            var layer = new Layer();
            Layers.Add(layer);

            for (var i = 0; i < size; i++)
            {
                var neuron = new Neuron();
                Inputs.ForEach(input => neuron.Inputs.Add(new InputConnection(input)));
                layer.AddNeuron(neuron);
            }
        }

        private void AddInnerLayer(int size)
        {
            var layer = new Layer();
            Layers.Add(layer);

            var prevLayer = Layers[Layers.Count - 2];
            for (var i = 0; i < size; i++)
            {
                var neuron = new Neuron();
                prevLayer.Neurons.ForEach(prevNeuron => neuron.Inputs.Add(new Connection(prevNeuron)));
                layer.AddNeuron(neuron);
            }
        }

        public List<double> GetResults()
        {
            Layers.ForEach(layer => layer.Neurons.ForEach(neuron => neuron.ResetOutput()));

            var lastLayer = Layers.Last();
            var results = lastLayer.Neurons.Select(neuron => neuron.Output);
            return results.ToList();
        }
    }
}
