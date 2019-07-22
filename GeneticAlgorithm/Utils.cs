using System;

namespace GeneticAlgorithm
{
    public static class Utils
    {
        public static Random Random { get; } = new Random(DateTime.Now.Millisecond);

        public static uint GetRandomUInt()
        {
            var thirtyBits = (uint)Random.Next(1 << 30);
            var twoBits = (uint)Random.Next(1 << 2);

            return (thirtyBits << 2) | twoBits;
        }
    }
}
