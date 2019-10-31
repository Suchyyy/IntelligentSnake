using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake.SnakeLogic
{
    public class Snake
    {
        public NeuralNetwork.NeuralNetwork Brain { get; set; }

        public List<Node> Nodes { get; }
        public List<double> Inputs { get; }

        public int Score { get; private set; }
        public int MovesLeft { get; set; }

        private readonly Board _board;

        private Direction _direction;
        public Direction Direction
        {
            get => _direction;
            private set { if (_direction.DirValue + value.DirValue != 0) _direction = value; }
        }

        public Snake(int size, Board board)
        {
            MovesLeft = 500;
            Score = 0;
            Nodes = new List<Node>();
            Inputs = new List<double>(new double[12]);
            _board = board;
            _direction = DirectionLeft.Instance;

            for (var i = 0; i < size; i++)
            {
                Nodes.Add(new Node
                {
                    Y = _board.Height / 2,
                    X = (_board.Width + size) / 2 - i
                });
            }

            Brain = new NeuralNetwork.NeuralNetwork(12);
            Brain.AddLayer(60);
            Brain.AddLayer(60);
            Brain.AddLayer(4);
        }

        public void Move()
        {
            if (MovesLeft <= 0) return;

            var head = Nodes.First();

            CalculateDistances(head);
            Brain.SetInputValues(Inputs);

            var results = Brain.GetResults();
            var index = results.Max() > 0.99 ? results.IndexOf(results.Max()) : -1;

            Direction = Direction.ChangeDirection(index);

            for (var i = Nodes.Count - 1; i > 0; i--)
            {
                var prev = Nodes[i];
                var next = Nodes[i - 1];

                prev.X = next.X;
                prev.Y = next.Y;
            }

            Direction.Move(head, _board);
            MovesLeft--;
            Score++;

            if (head.X < 0 || head.Y < 0 || head.X >= _board.Width || head.Y >= _board.Height
                || Nodes.Skip(3).Count(node => node.X == head.X && node.Y == head.Y) != 0)
            {
                MovesLeft = 0;
                Score -= 500;
                return;
            }

            if (_board.IsFruitOnPlace(head.X, head.Y))
            {
                Nodes.Add(Direction.Previous(head));
                Score += 600;
                MovesLeft += 300;

                var fruit = _board.Fruit;
                do
                {
                    fruit.X = _board.Random.Next(_board.Width);
                    fruit.Y = _board.Random.Next(_board.Height);
                } while (!IsValidPosition(fruit));
            }
        }

        private void CalculateDistances(Node head)
        {
            Inputs[0] = head.X == 0 ? 1 : -1;
            Inputs[1] = head.Y == 0 ? 1 : -1;
            Inputs[2] = _board.Width - head.X == 1 ? 1 : -1;
            Inputs[3] = _board.Height - head.Y == 1 ? 1 : -1;

            var distances = _direction.GetBodyDistances(this);
            for (var i = 0; i < 4; i++) Inputs[4 + i] = distances[i];

            var fruit = _board.Fruit;
            Inputs[8] = head.X < fruit.X ? 1 : -1;
            Inputs[9] = head.Y < fruit.Y ? 1 : -1;
            Inputs[10] = head.X > fruit.X ? 1 : -1;
            Inputs[11] = head.Y > fruit.Y ? 1 : -1;
        }

        private bool IsValidPosition(Node node)
        {
            foreach (var n in Nodes)
            {
                if (n.X == node.X && n.Y == node.Y) return false;
            }

            return true;
        }
    }
}
