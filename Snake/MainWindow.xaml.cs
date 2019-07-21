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
using MoreLinq;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Algorithm _geneticAlgorithm;
        private GameObjects _gameObjects;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameElements(4);

            KeyDown += OnButtonKeyDown;
        }

        private void StartAlgorithm()
        {
            Task.Run(() =>
            {
                var snakeList = new List<GameInstance>();

                for (var i = 0; i < 2000; i++) snakeList.Add(new GameInstance(78, 41, 4));

                var genomeSize = 0;
                snakeList[0].Snake.Brain.Layers.ForEach(layer => layer.Neurons.ForEach(neuron => genomeSize += neuron.Inputs.Count));

                _geneticAlgorithm = new Algorithm(2000, genomeSize);
                _geneticAlgorithm.Selection = new SelectionTournament(_geneticAlgorithm.Population, 20);
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

                    while (true)
                    {
                        var snakes = snakeList.Where(instance => instance.Snake.MovesLeft > 0).ToList();
                        snakes.AsParallel().ForAll(instance => instance.Snake.Move());

                        if (snakes.Count == 0) break;

                        var snake = snakes[0];
                        GameScene.Dispatcher.Invoke(() => RenderFrame(snake));
                        LabelMovesLeft.Dispatcher.Invoke(() => LabelMovesLeft.Content = $"Moves left: {snake.Snake.MovesLeft}");
                        LabelScore.Dispatcher.Invoke(() => LabelScore.Content = $"Score: {snake.Snake.Score}");

                        Thread.Sleep(16);
                    }

                    foreach (var c in _geneticAlgorithm.Population.Select((chromosome, index) => new { index, chromosome }))
                    {
                        c.chromosome.Fitness = snakeList[c.index].Snake.Score;
                        snakeList[c.index] = new GameInstance(78, 41, 4);
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
                    Fill = Brushes.ForestGreen,
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
                var snakeNodeEllipse = new Ellipse
                {
                    Fill = Brushes.ForestGreen,
                    Width = 10,
                    Height = 10
                };

                _gameObjects.SnakeBody.Add(snakeNodeEllipse);
                GameScene.Children.Add(snakeNodeEllipse);
            }

            foreach (var element in gameInstance.Snake.Nodes.Select((node, index) => new { node, ellipse = _gameObjects.SnakeBody[index] }))
            {
                Canvas.SetTop(element.ellipse, element.node.Y * 10);
                Canvas.SetLeft(element.ellipse, element.node.X * 10);
            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    StartAlgorithm();
                    break;
            }
        }
    }
}
