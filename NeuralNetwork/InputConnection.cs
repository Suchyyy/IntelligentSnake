namespace NeuralNetwork
{
    public class InputConnection : Connection
    {
        private readonly Input _input;

        public InputConnection(Input input) : base(null)
        {
            _input = input;
        }

        public override double GetOutput() => _input.Value * Weight;
    }
}
