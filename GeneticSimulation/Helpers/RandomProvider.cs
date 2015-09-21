// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomProvider.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
// </copyright>
// <summary>
//   The random provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.Helpers
{
    using System;
    using System.Threading;

    /// <summary>
    ///     The random provider.
    /// </summary>
    internal static class RandomProvider
    {
        /// <summary>
        ///     The random wrapper.
        /// </summary>
        private static readonly ThreadLocal<Random> RandomWrapper =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        /// <summary>
        ///     The seed.
        /// </summary>
        private static int seed = Environment.TickCount;

        /// <summary>
        ///     Get thread-safe random instance.
        /// </summary>
        /// <returns>
        ///     The <see cref="Random" />.
        /// </returns>
        public static Random GetThreadRandom()
        {
            return RandomWrapper.Value;
        }
    }
}