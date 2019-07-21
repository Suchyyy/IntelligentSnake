using System.Collections.Generic;
using System.Collections.Specialized;

namespace GeneticAlgorithm.Crossovers
{
    public class OnePointCrossover : Crossover
    {
        public OnePointCrossover(List<Chromosome> population, double crossoverProbability)
        {
            CrossoverProbability = crossoverProbability;
            Population = population;
            FirstGenome = new BitVector32[Population.Count];
            SecondGenome = new BitVector32[Population.Count];
        }

        protected override void CrossChromosomes(Chromosome first, Chromosome second)
        {
            var index = Utils.Random.Next(first.Genome.Length * 32);

            for (var i = 0; i < first.Genome.Length * 32; i++)
            {
                var nthValue = i / 32;
                var nthBit = i % 32;
                FirstGenome[nthValue][nthBit] = i < index ? first.Genome[nthValue][nthBit] : second.Genome[nthValue][nthBit];
                SecondGenome[nthValue][nthBit] = i < index ? second.Genome[nthValue][nthBit] : first.Genome[nthValue][nthBit];
            }
        }
    }
}
