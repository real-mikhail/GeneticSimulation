// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumHelper.cs" company="">
//   
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
        ///     The create random gene.
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
        ///     The choose random gene.
        /// </summary>
        /// <param name="gene1">
        ///     The gene 1.
        /// </param>
        /// <param name="gene2">
        ///     The gene 2.
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