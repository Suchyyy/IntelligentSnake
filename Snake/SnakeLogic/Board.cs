using System;
using GeneticAlgorithm;

namespace Snake.SnakeLogic
{
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Node Fruit { get; set; }
        public Random Random { get; }

        public Board(int width, int height, int seed)
        {
            Width = width;
            Height = height;
            Random = new Random(seed);
            Fruit = new Node { X = Random.Next(Width), Y = Random.Next(Height) };
        }

        public bool IsFruitOnPlace(int x, int y)
        {
            return Fruit.X == x && Fruit.Y == y;
        }
    }
}
