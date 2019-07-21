using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Chromosome : ICloneable
    {
        public BitVector32[] Genome { get; }
        public double Fitness { get; set; }

        public Chromosome(int length)
        {
            Genome = new BitVector32[length];

            for (var i = 0; i < length; i++) Genome[i] = new BitVector32(Utils.Random.Next(int.MinValue, int.MaxValue));
        }

        public List<double> GetWeights() => Genome.Select(value => (double)value.Data / int.MaxValue).ToList();

        public object Clone() => MemberwiseClone();
    }
}
