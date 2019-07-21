using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace GeneticAlgorithm.Crossovers
{
    public class TwoPointCrossover : Crossover
    {
        public TwoPointCrossover(List<Chromosome> population, double crossoverProbability)
        {
            CrossoverProbability = crossoverProbability;
            Population = population;
            FirstGenome = new BitVector32[Population[0].Genome.Length];
            SecondGenome = new BitVector32[Population[0].Genome.Length];
        }

        protected override void CrossChromosomes(Chromosome first, Chromosome second)
        {
            var r1 = Utils.Random.Next(first.Genome.Length * 32);
            var r2 = Utils.Random.Next(first.Genome.Length * 32);

            if (r1 > r2)
            {
                var tmp = r1;
                r1 = r2;
                r2 = tmp;
            }

            for (var i = 0; i < first.Genome.Length * 32; i++)
            {
                var nthValue = i / 32;
                var nthBit = i % 32;
                FirstGenome[nthValue][nthBit] = i < r1 || i > r2 ? first.Genome[nthValue][nthBit] : second.Genome[nthValue][nthBit];
                SecondGenome[nthValue][nthBit] = i < r1 || i > r2 ? second.Genome[nthValue][nthBit] : first.Genome[nthValue][nthBit];
            }
        }
    }
}
