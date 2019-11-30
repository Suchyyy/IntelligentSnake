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
            var genomeLength = first.Genome.Length * 32;
            var start = 0;
            var end = Utils.Random.Next(first.Genome.Length);

            if (end > genomeLength * 0.5)
            {
                // same result, less iterations
                start = end - 1;
                end = genomeLength;
            }

            for (var i = start; i < end; i++)
            {
                var gen = i / 32;
                var bit = i - gen * 32;

                if (bit == 0 && (end / 32) != gen)
                {
                    var genome = first.Genome[gen];
                    first.Genome[gen] = second.Genome[gen];
                    second.Genome[gen] = genome;

                    i += 31;
                }
                else
                {
                    var mask = 1 << bit;

                    first.Genome[gen][mask] = second.Genome[gen][mask];
                    second.Genome[gen][mask] = first.Genome[gen][mask];
                }
            }
        }
    }
}
