using System.Collections.Generic;
using System.Windows.Shapes;

namespace Snake
{
    public class GameObjects
    {
        public Ellipse FruitEllipse { get; set; }
        public List<Ellipse> SnakeBody { get; } = new List<Ellipse>();
    }
}
