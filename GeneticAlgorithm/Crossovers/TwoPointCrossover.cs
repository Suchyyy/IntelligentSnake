using System.Collections.Generic;

namespace GeneticAlgorithm.Crossovers
{
    public class TwoPointCrossover : Crossover
    {
        public TwoPointCrossover(List<Chromosome> population, double crossoverProbability)
        {
            CrossoverProbability = crossoverProbability;
            Population = population;
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
                var firstGenome = i < r1 || i > r2 ? first.Genome[nthValue][nthBit] : second.Genome[nthValue][nthBit];
                var secondGenome = i < r1 || i > r2 ? second.Genome[nthValue][nthBit] : first.Genome[nthValue][nthBit];

                first.Genome[nthValue][nthBit] = firstGenome;
                second.Genome[nthValue][nthBit] = secondGenome;
            }
        }
    }
}
