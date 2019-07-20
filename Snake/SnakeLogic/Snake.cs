using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake.SnakeLogic
{
    class Snake
    {
        public List<Node> Nodes { get; }
        public List<double> Inputs { get; }
        private readonly Board _board;
        private Direction _direction;

        public Direction Direction
        {
            get => _direction;
            set { if (_direction.DirValue + value.DirValue != 0) _direction = value; }
        }

        public Snake(int size, Board board)
        {
            Nodes = new List<Node>();
            Inputs = new List<double>(new double[9]);
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
        }

        public void Move()
        {
            for (var i = Nodes.Count - 1; i > 0; i--)
            {
                var prev = Nodes[i];
                var next = Nodes[i - 1];

                prev.X = next.X;
                prev.Y = next.Y;
            }

            var head = Nodes.First();

            Direction.Move(head, _board);

            if (_board.IsFruitOnPlace(head.X, head.Y))
            {
                Nodes.Add(Direction.Previous(head));

                var fruit = _board.Fruit;
                do
                {
                    fruit.X = Board.Random.Next(_board.Width);
                    fruit.Y = Board.Random.Next(_board.Height);
                } while (!IsValidPosition(fruit));
            }

            CalculateDistances(head);
            Console.WriteLine(string.Join(", ", Inputs));
        }

        private void CalculateDistances(Node head)
        {
            Inputs[0] = head.Y; // distance to the upper wall
            Inputs[1] = head.X; // distance to the left wall
            Inputs[2] = _board.Height - head.Y; // distance to the bottom wall
            Inputs[3] = _board.Width - head.X; // distance to the right wall

            var distances = _direction.GetDistances(this);
            Inputs[4] = distances[0]; // distance to the body in the front of
            Inputs[5] = distances[1]; // distance to the body in the left of
            Inputs[6] = distances[2]; // distance to the body in the right of

            Inputs[7] = _board.Fruit.X - head.X; // horizontal distance to the fruit
            Inputs[8] = _board.Fruit.Y - head.Y; // vertical distance to the fruit
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
