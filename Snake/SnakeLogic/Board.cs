using GeneticAlgorithm;

namespace Snake.SnakeLogic
{
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Node Fruit { get; set; }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            Fruit = new Node { X = Utils.Random.Next(Width), Y = Utils.Random.Next(Height) };
        }

        public bool IsFruitOnPlace(int x, int y)
        {
            return Fruit.X == x && Fruit.Y == y;
        }
    }
}
