namespace Snake.SnakeLogic
{
    public class GameInstance
    {
        public Board Board { get; }
        public Snake Snake { get; }

        public GameInstance(int boardWidth, int boardHeight, int snakeSize, int seed)
        {
            Board = new Board(boardWidth, boardHeight, seed);
            Snake = new Snake(snakeSize, Board);
        }
    }
}
