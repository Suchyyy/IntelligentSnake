using System.Linq;

namespace Snake.SnakeLogic
{
    public abstract class Direction
    {
        public abstract int DirValue { get; }
        public abstract void Move(Node node, Board board);
        public abstract Node Previous(Node node);

        public double[] GetDistances(Snake snake) => new[] { UpperDistance(snake), BottomDistance(snake), LeftDistance(snake), RightDistance(snake) };

        public abstract double[] GetPossibleDirections();
        public abstract Direction ChangeDirection(int value);

        public double UpperDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(1).Count(node => node.X == head.X && node.Y + 1 == head.Y);
        }

        public double BottomDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(1).Count(node => node.X == head.X && node.Y - 1 == head.Y);
        }

        public double LeftDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(1).Count(node => node.Y == head.Y && node.X + 1 == head.X);
        }

        public double RightDistance(Snake snake)
        {
            var head = snake.Nodes.First();
            return snake.Nodes.Skip(1).Count(node => node.Y == head.Y && node.X - 1 == head.X);
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

        public override double[] GetPossibleDirections() => new[] { 1.0, 0.0, 1.0, 1.0 };

        public override Direction ChangeDirection(int value)
        {
            switch (value)
            {
                case 0:
                    return DirectionUp.Instance;
                case 1:
                    return DirectionLeft.Instance;
                case 2:
                    return DirectionRight.Instance;
            }

            return DirectionUp.Instance;
        }
    }

    class DirectionDown : Direction
    {
        public override int DirValue { get; } = -1;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionDown());

        private DirectionDown() { }

        public override void Move(Node node, Board board) => node.Y++;

        public override Node Previous(Node node) => new Node { X = node.X, Y = node.Y - 1 };

        public override double[] GetPossibleDirections() => new[] { 0.0, 1.0, 1.0, 1.0 };

        public override Direction ChangeDirection(int value)
        {
            switch (value)
            {
                case 0:
                    return DirectionDown.Instance;
                case 1:
                    return DirectionRight.Instance;
                case 2:
                    return DirectionLeft.Instance;
            }

            return DirectionDown.Instance;
        }
    }

    class DirectionLeft : Direction
    {
        public override int DirValue { get; } = 2;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionLeft());

        private DirectionLeft() { }

        public override void Move(Node node, Board board) => node.X--;

        public override Node Previous(Node node) => new Node { X = node.X + 1, Y = node.Y };

        public override double[] GetPossibleDirections() => new[] { 1.0, 1.0, 1.0, 0.0 };

        public override Direction ChangeDirection(int value)
        {
            switch (value)
            {
                case 0:
                    return DirectionLeft.Instance;
                case 1:
                    return DirectionDown.Instance;
                case 2:
                    return DirectionUp.Instance;
            }

            return DirectionLeft.Instance;
        }
    }

    class DirectionRight : Direction
    {
        public override int DirValue { get; } = -2;
        private static Direction _instance;
        public static Direction Instance => _instance ?? (_instance = new DirectionRight());

        private DirectionRight() { }

        public override void Move(Node node, Board board) => node.X++;

        public override Node Previous(Node node) => new Node { X = node.X - 1, Y = node.Y };

        public override double[] GetPossibleDirections() => new[] { 1.0, 1.0, 0.0, 1.0 };

        public override Direction ChangeDirection(int value)
        {
            switch (value)
            {
                case 0:
                    return DirectionRight.Instance;
                case 1:
                    return DirectionUp.Instance;
                case 2:
                    return DirectionDown.Instance;
            }

            return DirectionRight.Instance;
        }
    }
}
