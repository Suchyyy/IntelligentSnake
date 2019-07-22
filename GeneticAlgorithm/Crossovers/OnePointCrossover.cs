using System.Collections.Generic;

namespace GeneticAlgorithm.Crossovers
{
    public class OnePointCrossover : Crossover
    {
        public OnePointCrossover(List<Chromosome> population, double crossoverProbability)
        {
            CrossoverProbability = crossoverProbability;
            Population = population;
        }

        protected override void CrossChromosomes(Chromosome first, Chromosome second)
        {
            var index = Utils.Random.Next(first.Genome.Length * 32);

            for (var i = 0; i < first.Genome.Length * 32; i++)
            {
                var nthValue = i / 32;
                var nthBit = i % 32;
                var firstGenome = i < index ? first.Genome[nthValue][nthBit] : second.Genome[nthValue][nthBit];
                var secondGenome = i < index ? second.Genome[nthValue][nthBit] : first.Genome[nthValue][nthBit];

                first.Genome[nthValue][nthBit] = firstGenome;
                second.Genome[nthValue][nthBit] = secondGenome;
            }
        }
    }
}
