// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cache.cs" company="">
//   
// </copyright>
// <summary>
//   The cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    ///     The cache.
    /// </summary>
    internal static class Cache
    {
        /// <summary>
        ///     The sync.
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        ///     The store.
        /// </summary>
        private static readonly Dictionary<string, Tuple<int, object>> Store =
            new Dictionary<string, Tuple<int, object>>();

        /// <summary>
        ///     The clear.
        /// </summary>
        public static void Clear()
        {
            lock (Sync)
            {
                Store.Clear();
            }
        }

        /// <summary>
        ///     The put.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="age">
        ///     The age.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public static void Put(string key, int age, object value)
        {
            lock (Sync)
            {
                Debug.Assert(
                    !Store.ContainsKey(key) || Store[key].Item1 <= age,
                    "Cache already stores this item with bigger age");
                Store[key] = new Tuple<int, object>(age, value);
            }
        }

        /// <summary>
        ///     The get.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="age">
        ///     The age.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object Get(string key, int age)
        {
            lock (Sync)
            {
                Tuple<int, object> returnValue;
                if (Store.TryGetValue(key, out returnValue))
                {
                    if (returnValue.Item1 >= age)
                    {
                        return returnValue.Item2;
                    }
                }

                return null;
            }
        }
    }
}