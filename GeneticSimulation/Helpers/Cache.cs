// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cache.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
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
        ///     The sync object.
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        ///     The storage.
        /// </summary>
        private static readonly Dictionary<string, Tuple<int, object>> Store =
            new Dictionary<string, Tuple<int, object>>();

        /// <summary>
        ///     Clears storage
        /// </summary>
        public static void Clear()
        {
            lock (Sync)
            {
                Store.Clear();
            }
        }

        /// <summary>
        ///     Puts object to storage.
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
        ///     Gets cached value.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="age">
        ///     Required age of stored value.
        /// </param>
        /// <returns>
        ///     Cached value, null - if there is no appropriate value
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