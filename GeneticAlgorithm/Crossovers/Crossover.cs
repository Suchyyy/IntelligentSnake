namespace GeneticAlgorithm.Crossovers
{
    public abstract class Crossover : GeneticOperations
    {
        protected double CrossoverProbability;

        protected abstract void CrossChromosomes(Chromosome first, Chromosome second);

        public void CrossPopulation()
        {
            for (var i = 0; i < Population.Count; i++)
            {
                if (Utils.Random.NextDouble() > CrossoverProbability) continue;

                var ch1 = Population[Utils.Random.Next(Population.Count)];
                var ch2 = Population[Utils.Random.Next(Population.Count)];

                CrossChromosomes(ch1, ch2);
            }
        }
    }
}
