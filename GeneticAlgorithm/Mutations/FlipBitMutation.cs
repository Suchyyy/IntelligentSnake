using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Mutations
{
    public class FlipBitMutation : Mutation
    {
        public FlipBitMutation(List<Chromosome> population, double mutationProbability)
        {
            Population = population;
            MutationProbability = mutationProbability;
        }

        public override void MutatePopulation()
        {
            Parallel.ForEach(Population, ch =>
            {
                if (Utils.Random.NextDouble() > MutationProbability) return;

                var index = Utils.Random.Next(ch.Genome.Length * 32);
                ch.Genome[index / 32][index % 32] = !ch.Genome[index / 32][index % 32];
            });
        }
    }
}
