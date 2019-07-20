using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    class Neuron
    {
        private bool _isOutputSet;
        public List<Connection> Inputs { get; }

        private double _output;
        public double Output
        {
            get
            {
                if (_isOutputSet) return _output;

                var value = Inputs.Sum(conn => conn.GetOutput());
                _output = Activation(value);
                _isOutputSet = true;

                return _output;
            }
        }

        public Neuron()
        {
            _isOutputSet = false;
            _output = -1.0;
            Inputs = new List<Connection>();
        }

        public void ResetOutput() => _isOutputSet = false;

        public double Activation(double value)
        {
            if (value > 1.0) return 1.0;
            if (value < 1.0) return -1.0;
            return _output;
        }
    }
}
