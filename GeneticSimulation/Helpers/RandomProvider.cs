// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomProvider.cs" company="">
//   
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
    /// The random provider.
    /// </summary>
    internal static class RandomProvider
    {
        /// <summary>
        /// The random wrapper.
        /// </summary>
        private static readonly ThreadLocal<Random> RandomWrapper =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        /// <summary>
        /// The seed.
        /// </summary>
        private static int seed = Environment.TickCount;

        /// <summary>
        /// The get thread random.
        /// </summary>
        /// <returns>
        /// The <see cref="Random"/>.
        /// </returns>
        public static Random GetThreadRandom()
        {
            // TODO: Optimize it
            return RandomWrapper.Value;
        }
    }
}
