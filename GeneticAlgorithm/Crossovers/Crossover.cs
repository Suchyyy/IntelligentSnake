using System.Collections.Specialized;

namespace GeneticAlgorithm.Crossovers
{
    public abstract class Crossover : GeneticOperations
    {
        protected double CrossoverProbability;
        protected BitVector32[] FirstGenome;
        protected BitVector32[] SecondGenome;

        protected abstract void CrossChromosomes(Chromosome first, Chromosome second);

        public void CrossPopulation()
        {
            foreach (var ch1 in Population)
            {
                if (Utils.Random.NextDouble() > CrossoverProbability) continue;

                Chromosome ch2;
                do
                {
                    ch2 = Population[Utils.Random.Next(Population.Count)];
                } while (ch2.Equals(ch1));

                CrossChromosomes(ch1, ch2);
            }
        }
    }
}
