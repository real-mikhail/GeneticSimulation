// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionHelper.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
// </copyright>
// <summary>
//   The extension helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///     The extension helper.
    /// </summary>
    internal static class ExtensionHelper
    {
        /// <summary>
        ///     Shuffles list.
        /// </summary>
        /// <param name="list">
        ///     The list.
        /// </param>
        /// <typeparam name="T">
        ///     Type of items
        /// </typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            Contract.Requires<ArgumentNullException>(list != null);
            var rnd = RandomProvider.GetThreadRandom();

            // Fisher–Yates shuffle modern algorithm
            for (var i = list.Count - 1; i >= 1; i--)
            {
                list.Swap(i, rnd.Next(i));
            }
        }

        /// <summary>
        ///     Swaps to items in the list
        /// </summary>
        /// <param name="list">
        ///     The list.
        /// </param>
        /// <param name="index1">
        ///     The index of item 1.
        /// </param>
        /// <param name="index2">
        ///     The index of item 2.
        /// </param>
        /// <typeparam name="T">
        ///     Type of items
        /// </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            Debug.Assert(index1 >= 0 && index1 < list.Count && index2 >= 0 && index2 < list.Count, "Out of range");
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }
}