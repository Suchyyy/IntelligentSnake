namespace NeuralNetwork
{
    class Connection
    {
        public double Weight { get; set; }
        private readonly Neuron _inputNeuron;

        public Connection(Neuron inputNeuron)
        {
            _inputNeuron = inputNeuron;
        }

        public virtual double GetOutput() => _inputNeuron.Output * Weight;
    }
}
