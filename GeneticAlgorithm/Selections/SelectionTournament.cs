using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace GeneticAlgorithm.Selections
{
    public class SelectionTournament : Selection
    {
        private readonly int _tournamentSize;
        private readonly Chromosome[] _tournament;

        public SelectionTournament(List<Chromosome> population, int tournamentSize)
        {
            Population = population;
            NewPopulation = new Chromosome[population.Count];
            _tournamentSize = tournamentSize;
            _tournament = new Chromosome[_tournamentSize];
        }

        public override void SelectPopulation()
        {
            NewPopulation[0] = Population.MaxBy(chromosome => chromosome.Fitness).First();

            for (var i = 1; i < Population.Count; i++)
            {
                for (var j = 0; j < _tournamentSize; j++)
                {
                    _tournament[j] = Population[Utils.Random.Next(Population.Count)];
                }

                NewPopulation[i] = _tournament.MaxBy(ch => ch.Fitness).First();
            }

            NewPopulation.AsParallel().ForEach((chromosome, index) => Population[index] = chromosome);
        }
    }
}
