using System.Collections.Generic;
using System.Linq;

namespace Snake.SnakeLogic
{
    public abstract class Direction
    {
        public abstract int DirValue { get; }
        public abstract void Move(Node node, Board board);
        public abstract Node Previous(Node node);

        public List<double> GetDistances(Snake snake)
        {
            return new List<double>(new[] { UpperDistance(snake), BottomDistance(snake), LeftDistance(snake), RightDistance(snake) });
        }

        public double UpperDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes
                .Skip(1)
                .Where(node => node.X == head.X && node.Y < head.Y)
                .DefaultIfEmpty(new Node { Y = head.Y + 1 })
                .Min(node => head.Y - node.Y);
        }

        public double BottomDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes
                .Skip(1)
                .Where(node => node.X == head.X && node.Y > head.Y)
                .DefaultIfEmpty(new Node { Y = head.Y - 1 })
                .Min(node => node.Y - head.Y);
        }

        public double LeftDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes
                .Skip(1)
                .Where(node => node.Y == head.Y && node.X < head.X)
                .DefaultIfEmpty(new Node { X = head.X + 1 })
                .Min(node => head.X - node.X);
        }

        public double RightDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes
                .Skip(1)
                .Where(node => node.Y == head.Y && node.X > head.X)
                .DefaultIfEmpty(new Node { X = head.X - 1 })
                .Min(node => node.X - head.X);
        }
    }

    class DirectionUp : Direction
    {
        public override int DirValue { get; } = 1;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionUp());

        private DirectionUp() { }

        public override void Move(Node node, Board board)
        {
            node.Y--;
        }

        public override Node Previous(Node node)
        {
            return new Node { X = node.X, Y = node.Y + 1 };
        }
    }

    class DirectionDown : Direction
    {
        public override int DirValue { get; } = -1;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionDown());

        private DirectionDown() { }

        public override void Move(Node node, Board board)
        {
            node.Y++;
        }

        public override Node Previous(Node node)
        {
            return new Node { X = node.X, Y = node.Y - 1 };
        }
    }

    class DirectionLeft : Direction
    {
        public override int DirValue { get; } = 2;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionLeft());

        private DirectionLeft() { }

        public override void Move(Node node, Board board)
        {
            node.X--;
        }

        public override Node Previous(Node node)
        {
            return new Node { X = node.X + 1, Y = node.Y };
        }
    }

    class DirectionRight : Direction
    {
        public override int DirValue { get; } = -2;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionRight());

        private DirectionRight() { }

        public override void Move(Node node, Board board)
        {
            node.X++;
        }

        public override Node Previous(Node node)
        {
            return new Node { X = node.X - 1, Y = node.Y };
        }
    }
}
