using System.Linq;

namespace Snake.SnakeLogic
{
    public abstract class Direction
    {
        public abstract int DirValue { get; }
        public abstract void Move(Node node, Board board);
        public abstract Node Previous(Node node);

        public double[] GetBodyDistances(Snake snake) => new[] { UpperDistance(snake), BottomDistance(snake), LeftDistance(snake), RightDistance(snake) };

        public abstract double[] GetPossibleDirections();

        public Direction ChangeDirection(int value)
        {
            Direction d = null;

            switch (value)
            {
                case 0:
                    d = DirectionUp.Instance;
                    break;
                case 1:
                    d = DirectionDown.Instance;
                    break;
                case 2:
                    d = DirectionLeft.Instance;
                    break;
                case 3:
                    d = DirectionRight.Instance;
                    break;
                default:
                    return this;
            }

            return DirValue + d.DirValue != 0 ? d : this;
        }

        public double UpperDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(3).Count(node => node.X == head.X && node.Y + 1 == head.Y) > 0 ? 1 : -1;
        }

        public double BottomDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(3).Count(node => node.X == head.X && node.Y - 1 == head.Y) > 0 ? 1 : -1;
        }

        public double LeftDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(3).Count(node => node.Y == head.Y && node.X + 1 == head.X) > 0 ? 1 : -1;
        }

        public double RightDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(3).Count(node => node.Y == head.Y && node.X - 1 == head.X) > 0 ? 1 : -1;
        }
    }

    class DirectionUp : Direction
    {
        public override int DirValue { get; } = 1;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionUp());

        private DirectionUp() { }

        public override void Move(Node node, Board board) => node.Y--;

        public override Node Previous(Node node) => new Node { X = node.X, Y = node.Y + 1 };

        public override double[] GetPossibleDirections() => new[] { 1.0, -1.0, 1.0, 1.0 };
    }

    class DirectionDown : Direction
    {
        public override int DirValue { get; } = -1;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionDown());

        private DirectionDown() { }

        public override void Move(Node node, Board board) => node.Y++;

        public override Node Previous(Node node) => new Node { X = node.X, Y = node.Y - 1 };

        public override double[] GetPossibleDirections() => new[] { -1.0, 1.0, 1.0, 1.0 };

    }

    class DirectionLeft : Direction
    {
        public override int DirValue { get; } = 2;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionLeft());

        private DirectionLeft() { }

        public override void Move(Node node, Board board) => node.X--;

        public override Node Previous(Node node) => new Node { X = node.X + 1, Y = node.Y };

        public override double[] GetPossibleDirections() => new[] { 1.0, 1.0, 1.0, -1.0 };
    }

    class DirectionRight : Direction
    {
        public override int DirValue { get; } = -2;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionRight());

        private DirectionRight() { }

        public override void Move(Node node, Board board) => node.X++;

        public override Node Previous(Node node) => new Node { X = node.X - 1, Y = node.Y };

        public override double[] GetPossibleDirections() => new[] { 1.0, 1.0, -1.0, 1.0 };
    }
}
