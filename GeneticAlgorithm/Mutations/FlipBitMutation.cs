using System.Collections.Generic;

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
            foreach (var ch in Population)
            {
                if (Utils.Random.NextDouble() > MutationProbability) continue;

                var index = Utils.Random.Next(ch.Genome.Length * 32);
                ch.Genome[index / 32][index % 32] = !ch.Genome[index / 32][index % 32];
            }
        }
    }
}
