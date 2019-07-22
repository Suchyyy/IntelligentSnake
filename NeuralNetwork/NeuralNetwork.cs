using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public List<Input> Inputs { get; }
        public List<Layer> Layers { get; }

        public NeuralNetwork(int inputCount)
        {
            Layers = new List<Layer>();
            Inputs = new List<Input>();

            for (var i = 0; i < inputCount; i++) Inputs.Add(new Input(0.0));
        }

        public void SetInputValues(List<double> inputs)
        {
            for (var i = 0; i < Inputs.Count; i++) Inputs[i].Value = inputs[i];
        }

        public void AddLayer(int size)
        {
            var layer = new Layer();
            Layers.Add(layer);

            if (Layers.Count > 1)
            {
                var prevLayer = Layers[Layers.Count - 2];
                for (var i = 0; i < size; i++)
                {
                    var neuron = new Neuron();
                    prevLayer.Neurons.ForEach(prevNeuron => neuron.Inputs.Add(new Connection(prevNeuron)));
                    layer.AddNeuron(neuron);
                }
            }
            else
            {
                for (var i = 0; i < size; i++)
                {
                    var neuron = new Neuron();
                    Inputs.ForEach(input => neuron.Inputs.Add(new InputConnection(input)));
                    layer.AddNeuron(neuron);
                }
            }
        }

        public List<double> GetResults()
        {
            Layers.ForEach(layer => layer.Neurons.ForEach(neuron => neuron.ResetOutput()));

            foreach (var layer in Layers)
            {
                layer.CalculateOutput();
            }

            var lastLayer = Layers.Last();
            var results = lastLayer.Neurons.Select(neuron => neuron.Output);
            return results.ToList();
        }
    }
}
