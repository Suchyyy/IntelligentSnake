using System;

namespace GeneticAlgorithm
{
    public static class Utils
    {
        private static readonly object Locker = new object();
        private static Random _random;
        public static Random Random
        {
            get
            {
                lock (Locker) { return _random ?? (_random = new Random(DateTime.Now.Millisecond)); }
            }
        }

        public static uint GetRandomUInt()
        {
            var thirtyBits = (uint)Random.Next(1 << 30);
            var twoBits = (uint)Random.Next(1 << 2);

            return (thirtyBits << 2) | twoBits;
        }
    }
}
