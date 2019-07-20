using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Snake.SnakeLogic;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Board _board;
        private readonly SnakeLogic.Snake _snake;
        private readonly GameObjects _gameObjects;

        public MainWindow()
        {
            InitializeComponent();

            _board = new Board(78, 41);
            _snake = new SnakeLogic.Snake(4, _board);
            _gameObjects = new GameObjects();

            InitializeGameElements();

            var timer = new DispatcherTimer();
            timer.Tick += RenderFrame;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            timer.Start();

            KeyDown += OnButtonKeyDown;
        }

        private void InitializeGameElements()
        {
            var fruitEllipse = new Ellipse
            {
                Fill = Brushes.Red,
                Width = 10,
                Height = 10
            };

            _gameObjects.FruitEllipse = fruitEllipse;
            GameScene.Children.Add(fruitEllipse);

            for (var i = 0; i < _snake.Nodes.Count; i++)
            {
                var snakeNodeEllipse = new Ellipse
                {
                    Fill = Brushes.ForestGreen,
                    Width = 10,
                    Height = 10
                };

                _gameObjects.SnakeBody.Add(snakeNodeEllipse);
                GameScene.Children.Add(snakeNodeEllipse);
            }
        }

        private void RenderFrame(object sender, EventArgs e)
        {
            _snake.Move();

            Canvas.SetTop(_gameObjects.FruitEllipse, _board.Fruit.Y * 10);
            Canvas.SetLeft(_gameObjects.FruitEllipse, _board.Fruit.X * 10);

            for (var i = 0; i < _snake.Nodes.Count - _gameObjects.SnakeBody.Count; i++)
            {
                var snakeNodeEllipse = new Ellipse
                {
                    Fill = Brushes.ForestGreen,
                    Width = 10,
                    Height = 10
                };

                _gameObjects.SnakeBody.Add(snakeNodeEllipse);
                GameScene.Children.Add(snakeNodeEllipse);
            }

            foreach (var element in _gameObjects.SnakeBody.Select((ellipse, index) => new { ellipse, node = _snake.Nodes[index] }))
            {
                Canvas.SetTop(element.ellipse, element.node.Y * 10);
                Canvas.SetLeft(element.ellipse, element.node.X * 10);
            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    _snake.Direction = DirectionUp.Instance;
                    break;

                case Key.S:
                case Key.Down:
                    _snake.Direction = DirectionDown.Instance;
                    break;

                case Key.A:
                case Key.Left:
                    _snake.Direction = DirectionLeft.Instance;
                    break;

                case Key.D:
                case Key.Right:
                    _snake.Direction = DirectionRight.Instance;
                    break;
            }
        }
    }
}
