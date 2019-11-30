using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Chromosome
    {
        public BitVector32[] Genome { get; }
        public double Fitness { get; set; }

        public Chromosome(int length)
        {
            Genome = new BitVector32[length];

            for (var i = 0; i < length; i++) Genome[i] = new BitVector32(Utils.Random.Next(int.MinValue, int.MaxValue));
        }

        public List<double> GetWeights() => Genome.Select(value => value.Data / 1000000.0).ToList();

        public Chromosome Clone()
        {
            Chromosome c = new Chromosome(Genome.Length) { Fitness = Fitness };
            for (int i = 0; i < Genome.Length; i++) c.Genome[i] = new BitVector32(Genome[i]);

            return c;
        }
    }
}
