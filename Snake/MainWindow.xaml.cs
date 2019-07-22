using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Snake.SnakeLogic;
using GeneticAlgorithm;
using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Mutations;
using GeneticAlgorithm.Selections;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _seed;
        private Algorithm _geneticAlgorithm;
        private GameObjects _gameObjects;
        private int _delay;

        public MainWindow()
        {
            _seed = DateTime.Now.Millisecond;
            _delay = 8;

            InitializeComponent();
            InitializeGameElements(4);

            KeyDown += OnButtonKeyDown;
        }

        private void StartAlgorithm()
        {
            Task.Run(() =>
            {
                var snakeList = new List<GameInstance>();

                for (var i = 0; i < 1000; i++) snakeList.Add(new GameInstance(77, 40, 4, _seed));

                var genomeSize = 0;
                snakeList[0].Snake.Brain.Layers.ForEach(layer => layer.Neurons.ForEach(neuron => genomeSize += neuron.Inputs.Count));

                _geneticAlgorithm = new Algorithm(1000, genomeSize);
                _geneticAlgorithm.Selection = new SelectionTournament(_geneticAlgorithm.Population, 100);
                _geneticAlgorithm.Crossover = new TwoPointCrossover(_geneticAlgorithm.Population, 0.65);
                _geneticAlgorithm.Mutation = new FlipBitMutation(_geneticAlgorithm.Population, 0.05);

                var generation = 1;
                while (true)
                {
                    LabelGeneration.Dispatcher.Invoke(() => LabelGeneration.Content = $"Generation: {generation++}");
                    foreach (var c in _geneticAlgorithm.Population.Select((chromosome, index) => new { index, weights = chromosome.GetWeights() }).AsParallel())
                    {
                        var i = 0;
                        snakeList[c.index].Snake.Brain.Layers
                            .ForEach(layer => layer.Neurons
                                .ForEach(neuron => neuron.Inputs
                                    .ForEach(connection => connection.Weight = c.weights[i++])));
                    }

                    _delay = 8;
                    var snake = snakeList[0];
                    while (true)
                    {
                        var snakes = snakeList.Where(instance => instance.Snake.MovesLeft > 0).ToList();
                        snakes.AsParallel().ForAll(instance => instance.Snake.Move());

                        if (snakes.Count == 0) break;

                        GameScene.Dispatcher.Invoke(() => RenderFrame(snake));
                        LabelMovesLeft.Dispatcher.Invoke(() => LabelMovesLeft.Content = $"Moves left: {snake.Snake.MovesLeft}");
                        LabelScore.Dispatcher.Invoke(() => LabelScore.Content = $"Score: {snake.Snake.Score}");

                        if (snake.Snake.MovesLeft > 0) Thread.Sleep(_delay);
                    }

                    _seed = DateTime.Now.Millisecond;
                    foreach (var c in _geneticAlgorithm.Population.Select((chromosome, index) => new { index, chromosome }))
                    {
                        c.chromosome.Fitness = snakeList[c.index].Snake.Score;
                        snakeList[c.index] = new GameInstance(77, 40, 4, _seed);
                    }

                    _geneticAlgorithm.NextGeneration();
                }
            });
        }

        private void InitializeGameElements(int snakeSize)
        {
            _gameObjects = new GameObjects();
            var fruitEllipse = new Ellipse
            {
                Fill = Brushes.Red,
                Width = 10,
                Height = 10
            };

            _gameObjects.FruitEllipse = fruitEllipse;
            GameScene.Children.Add(fruitEllipse);

            for (var i = 0; i < snakeSize; i++)
            {
                var snakeNodeEllipse = new Ellipse
                {
                    Fill = i == 0 ? Brushes.DarkGreen : Brushes.ForestGreen,
                    Width = 10,
                    Height = 10
                };

                _gameObjects.SnakeBody.Add(snakeNodeEllipse);
                GameScene.Children.Add(snakeNodeEllipse);
            }
        }

        private void RenderFrame(GameInstance gameInstance)
        {
            Canvas.SetTop(_gameObjects.FruitEllipse, gameInstance.Board.Fruit.Y * 10);
            Canvas.SetLeft(_gameObjects.FruitEllipse, gameInstance.Board.Fruit.X * 10);

            for (var i = 0; i < gameInstance.Snake.Nodes.Count - _gameObjects.SnakeBody.Count; i++)
            {
                var ellipse = new Ellipse
                {
                    Fill = Brushes.ForestGreen,
                    Width = 10,
                    Height = 10
                };

                _gameObjects.SnakeBody.Add(ellipse);
                GameScene.Children.Add(ellipse);

                Canvas.SetTop(ellipse, -100);
                Canvas.SetLeft(ellipse, -100);
            }

            for (var i = 0; i < _gameObjects.SnakeBody.Count; i++)
            {
                var ellipse = _gameObjects.SnakeBody[i];
                if (i < gameInstance.Snake.Nodes.Count)
                {
                    var node = gameInstance.Snake.Nodes[i];
                    Canvas.SetTop(ellipse, node.Y * 10);
                    Canvas.SetLeft(ellipse, node.X * 10);
                }
                else
                {
                    Canvas.SetTop(ellipse, -100);
                    Canvas.SetLeft(ellipse, -100);
                }
            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    StartAlgorithm();
                    break;

                case Key.LeftCtrl:
                    _delay = 1;
                    break;
            }
        }
    }
}
