using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Snake.SnakeLogic;

namespace Snake
{
    class GameObjects
    {
        public Ellipse FruitEllipse { get; set; }
        public List<Ellipse> SnakeBody { get; } = new List<Ellipse>();
    }
}
