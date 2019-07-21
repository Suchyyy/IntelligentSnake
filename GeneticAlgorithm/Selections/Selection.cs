namespace GeneticAlgorithm.Selections
{
    public abstract class Selection : GeneticOperations
    {
        protected Chromosome[] NewPopulation;
        public abstract void SelectPopulation();
    }
}
