using System;

namespace Snake.SnakeLogic
{
    public class Board
    {
        private static readonly object Locker = new object();
        private static Random _random;
        public static Random Random
        {
            get
            {
                lock (Locker)
                {
                    return _random ?? (_random = new Random(DateTime.Now.Millisecond));
                }
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public Node Fruit { get; set; }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Fruit = new Node { X = Random.Next(Width), Y = Random.Next(Height) };
        }

        public bool IsFruitOnPlace(int x, int y)
        {
            return Fruit.X == x && Fruit.Y == y;
        }
    }
}
