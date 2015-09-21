// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumHelper.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
// </copyright>
// <summary>
//   The enum helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.Helpers
{
    using System.Runtime.CompilerServices;

    using MZ.GeneticSimulation.DataModel;

    /// <summary>
    ///     The enum helper.
    /// </summary>
    internal static class EnumHelper
    {
        /// <summary>
        ///     Creates random gene.
        /// </summary>
        /// <returns>
        ///     The <see cref="Gene" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Gene CreateRandomGene()
        {
            return (Gene)(1 << RandomProvider.GetThreadRandom().Next(3));
        }

        /// <summary>
        ///     Chooses random gene.
        /// </summary>
        /// <param name="gene1">
        ///     Option 1.
        /// </param>
        /// <param name="gene2">
        ///     Option 2.
        /// </param>
        /// <returns>
        ///     The <see cref="Gene" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Gene ChooseRandomGene(Gene gene1, Gene gene2)
        {
            return RandomProvider.GetThreadRandom().NextDouble() < 0.5 ? gene1 : gene2;
        }
    }
}