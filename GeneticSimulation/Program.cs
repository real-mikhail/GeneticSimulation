// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
// </copyright>
// <summary>
//   Main program (entry class).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using MZ.GeneticSimulation.DataModel;

    using static System.Console;
    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    ///     The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     Iteration number
        /// </summary>
        private const int IterationsNumber = 128;

        /// <summary>
        ///     Entry point.
        /// </summary>
        private static void Main()
        {
            // TODO: Fix comments, indentation and other stuff
            var begin = DateTime.UtcNow;
            var previous = DateTime.UtcNow;
            var universe = new World();
            var list = new List<Statistic>(new[] { universe.Statistic });
            list.AddRange(
                Enumerable.Range(1, IterationsNumber).Select(
                    i =>
                        {
                            universe.Run(8);
                            var current = DateTime.UtcNow;
                            WriteLine(
                                $"[{(current - begin).TotalSeconds:000.00}|+{(current - previous).TotalSeconds:000.00}]\t<=-\t{i}/{IterationsNumber}\t-=>");
                            previous = current;
                            return universe.Statistic;
                        }));

            Write(Environment.NewLine);
            PrintPopulationInfo(list);
            PrintGenesInfo(list);
            ForegroundColor = ConsoleColor.White;
            Write(Environment.NewLine);
            Write(Environment.NewLine);
            Write(Environment.NewLine);
            WriteLine("Press enter for exit...");
            ReadLine();
        }

        /// <summary>
        ///     Prints population info.
        /// </summary>
        /// <param name="statistics">
        ///     The statistics.
        /// </param>
        private static void PrintPopulationInfo(List<Statistic> statistics)
        {
            Requires<ArgumentNullException>(statistics != null);
            Requires<ArgumentException>(statistics.Count > 0, "There is no any statistic. WTF!");
            ForegroundColor = ConsoleColor.Yellow;

            // Print header
            WriteLine("\t\t\tPOPULATION");
            Write("Age");
            for (var i = 0; i < statistics[0].SpeciesNumber; i++)
            {
                Write("\t|{0}", i);
            }

            // Print data
            foreach (var statistic in statistics)
            {
                Write(Environment.NewLine);
                var populationInfo = statistic.PopulationInfo;
                Debug.Assert(
                    populationInfo.Length == statistic.SpeciesNumber,
                    "There is no information about some species");
                Write(statistic.Age);
                foreach (var number in populationInfo)
                {
                    Write("\t|{0}", number);
                }
            }

            Write(Environment.NewLine);
        }

        /// <summary>
        ///     Prints genes info.
        /// </summary>
        /// <param name="statistics">
        ///     The statistics.
        /// </param>
        private static void PrintGenesInfo(List<Statistic> statistics)
        {
            Requires<ArgumentNullException>(statistics != null);
            Requires<ArgumentException>(statistics.Count > 0, "There is no any statistic. WTF!");
            ForegroundColor = ConsoleColor.Yellow;

            // Print header
            Write(Environment.NewLine);
            WriteLine("\t\t\tGENES");
            WriteLine("Age\t|SelfishGene\t|AltruisticGene\t|CreatureLevelGene");

            // Print data
            foreach (var statistic in statistics)
            {
                WriteLine(
                    "{0}\t|{1:f4}\t\t|{2:f4}\t\t|{3:f4}",
                    statistic.Age,
                    statistic.SelfishPercentPerCreature,
                    statistic.AltruisticPercentPerCreature,
                    statistic.CreatureLevelGenesPercentPerCreature);
            }
        }
    }
}