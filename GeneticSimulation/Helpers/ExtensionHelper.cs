// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The extension helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeneticSimulation.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The extension helper.
    /// </summary>
    internal static class ExtensionHelper
    {
        /// <summary>
        /// The shuffle.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            // Fisher–Yates shuffle modern algorithm
            for (int i = list.Count - 1; i >= 1; i--)
            {
                list.Swap(i, MathHelper.Rnd.Next(i));
            }
        }

        /// <summary>
        /// The swap.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="index1">
        /// The index 1.
        /// </param>
        /// <param name="index2">
        /// The index 2.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            Debug.Assert(index1 > 0 && index1 < list.Count && index2 > 0 && index2 < list.Count, "Out of range");
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }
}
