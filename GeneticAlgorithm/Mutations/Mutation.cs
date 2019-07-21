namespace GeneticAlgorithm.Mutations
{
    public abstract class Mutation : GeneticOperations
    {
        protected double MutationProbability;
        public abstract void MutatePopulation();
    }
}
