using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace GeneticAlgorithm.Selections
{
    public class SelectionTournament : Selection
    {
        private readonly int _tournamentSize;

        public SelectionTournament(List<Chromosome> population, int tournamentSize)
        {
            Population = population;
            NewPopulation = new Chromosome[population.Count];
            _tournamentSize = tournamentSize;
        }

        public override void SelectPopulation()
        {
            NewPopulation[0] = Population.MaxBy(chromosome => chromosome.Fitness).First();

            Parallel.For(1, Population.Count, i =>
            {
                var tournament = new Chromosome[_tournamentSize];

                for (var j = 0; j < _tournamentSize; j++)
                {
                    tournament[j] = Population[Utils.Random.Next(Population.Count)];
                }

                NewPopulation[i] = tournament.MaxBy(ch => ch.Fitness).First();
            });

            NewPopulation.AsParallel().ForEach((chromosome, index) => Population[index] = (Chromosome)chromosome.Clone());
        }
    }
}
