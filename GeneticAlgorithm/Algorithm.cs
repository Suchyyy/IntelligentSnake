using System.Collections.Generic;
using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Mutations;
using GeneticAlgorithm.Selections;

namespace GeneticAlgorithm
{
    public class Algorithm
    {
        public List<Chromosome> Population { get; }
        public Selection Selection { get; set; }
        public Crossover Crossover { get; set; }
        public Mutation Mutation { get; set; }

        public Algorithm(int populationSize, int genomeSize)
        {
            Population = new List<Chromosome>();

            for (var i = 0; i < populationSize; i++) Population.Add(new Chromosome(genomeSize));
        }

        public void NextGeneration()
        {
            Selection.SelectPopulation();
            Crossover.CrossPopulation();
            Mutation.MutatePopulation();
        }
    }
}
