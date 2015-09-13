// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MZ.GeneticSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using static System.Console;
    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            // TODO: Fix comments, indentation and other stuff
            // TODO: Fix GC hell (reduce memory traffic)
            var universe = new World();
            var list = new List<Statistic>(new[] { universe.Statistic });
            list.AddRange(Enumerable.Range(1, 128).Select(
                _ =>
                    {
                        universe.Run(8);
                        Write("-=>");
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
        /// The print population info.
        /// </summary>
        /// <param name="statistics">
        /// The statistics.
        /// </param>
        private static void PrintPopulationInfo(List<Statistic> statistics)
        {
            Requires<ArgumentNullException>(statistics != null);
            Requires<ArgumentException>(statistics.Count > 0, "There is no any statistic. WTF!");
            ForegroundColor = ConsoleColor.Blue;
            
            // Print header
            WriteLine("\t\t\tPOPULATION");
            Write("Age");
            for (int i = 0; i < statistics[0].SpeciesNumber; i++)
            {
                Write("\t|{0}", i);
            }

            // Print data
            foreach (Statistic statistic in statistics)
            {
                Write(Environment.NewLine);
                int[] populationInfo = statistic.PopulationInfo;
                Debug.Assert(populationInfo.Length == statistic.SpeciesNumber, "There is no information about some species");
                Write(statistic.Age);
                foreach (int number in populationInfo)
                {
                    Write("\t|{0}", number);
                }
            }

            Write(Environment.NewLine);
        }

        /// <summary>
        /// The print genes info.
        /// </summary>
        /// <param name="statistics">
        /// The statistics.
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
            foreach (Statistic statistic in statistics)
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
