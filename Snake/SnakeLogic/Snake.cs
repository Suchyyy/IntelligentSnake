using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;

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
            MovesLeft = 600;
            Score = 0;
            Nodes = new List<Node>();
            Inputs = new List<double>(new double[8]);
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

            Brain = new NeuralNetwork.NeuralNetwork(8);
            Brain.AddLayer(8);
            Brain.AddLayer(8);
            Brain.AddLayer(4);
        }

        public void Move()
        {
            if (MovesLeft <= 0) return;

            Brain.SetInputValues(Inputs);
            var results = Brain.GetResults(); // [UP, DOWN, LEFT, RIGHT]
            SetDirection(results.IndexOf(1.0));

            for (var i = Nodes.Count - 1; i > 0; i--)
            {
                var prev = Nodes[i];
                var next = Nodes[i - 1];

                prev.X = next.X;
                prev.Y = next.Y;
            }

            var head = Nodes.First();

            Direction.Move(head, _board);
            MovesLeft--;

            if (head.X < 0 || head.Y < 0 || head.X >= _board.Width || head.Y >= _board.Height)
            {
                MovesLeft = 0;
                return;
            }

            if (_board.IsFruitOnPlace(head.X, head.Y))
            {
                Nodes.Add(Direction.Previous(head));
                Score += 1000;
                MovesLeft += 601;

                var fruit = _board.Fruit;
                do
                {
                    fruit.X = Utils.Random.Next(_board.Width);
                    fruit.Y = Utils.Random.Next(_board.Height);
                } while (!IsValidPosition(fruit));
            }

            CalculateDistances(head);
        }

        private void CalculateDistances(Node head)
        {
            Inputs[0] = head.Y + 1; // distance to the upper wall
            Inputs[1] = head.X + 1; // distance to the left wall
            Inputs[2] = _board.Height - head.Y; // distance to the bottom wall
            Inputs[3] = _board.Width - head.X; // distance to the right wall

//            var distances = _direction.GetDistances(this);
//            Inputs[4] = distances[0];
//            Inputs[5] = distances[1];
//            Inputs[6] = distances[2];
//            Inputs[7] = distances[3];

            Inputs[4] = _board.Fruit.Y == head.Y ? 10.0 :
                _board.Fruit.Y - head.Y < 0.0 ? 0.0 : 5.0 / _board.Fruit.Y - head.Y;

            Inputs[5] = _board.Fruit.Y == head.Y ? 10.0 :
                head.Y - _board.Fruit.Y < 0.0 ? 0.0 : 5.0 / head.Y - _board.Fruit.Y;

            Inputs[6] = _board.Fruit.X == head.X ? 10.0 :
                _board.Fruit.X - head.X < 0.0 ? 0.0 : 5.0 / _board.Fruit.X - head.X;

            Inputs[7] = _board.Fruit.X == head.X ? 10.0 :
                head.X - _board.Fruit.X < 0.0 ? 0.0 : 5.0 / head.X - _board.Fruit.X;
        }

        private void SetDirection(int index)
        {
            switch (index)
            {
                case 0:
                    Direction = DirectionUp.Instance;
                    break;
                case 1:
                    Direction = DirectionDown.Instance;
                    break;
                case 2:
                    Direction = DirectionLeft.Instance;
                    break;
                case 3:
                    Direction = DirectionRight.Instance;
                    break;
            }
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
