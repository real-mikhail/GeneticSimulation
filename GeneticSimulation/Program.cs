// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GeneticSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

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
            // TODO: Use contracts everywhere?
            // TODO: Fix comments and other stuff
            // TODO: Fix GC hell (reduce memory traffic)
            var universe = new World();
            var list = Enumerable.Range(1, 8).Select(
                _ =>
                    {
                        universe.Run(13);
                        Console.Write("-=>");
                        return universe.Statistic;
                    }).ToList();

            Console.Write(Environment.NewLine);
            PrintPopulationInfo(list);
            PrintGenesInfo(list);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine);
            Console.Write(Environment.NewLine);
            Console.Write(Environment.NewLine);
            Console.WriteLine("Press enter for exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// The print population info.
        /// </summary>
        /// <param name="statistics">
        /// The statistics.
        /// </param>
        private static void PrintPopulationInfo(List<Statistic> statistics)
        {
            Debug.Assert(statistics.Count > 0, "There is no any statistic. WTF!");
            Console.ForegroundColor = ConsoleColor.Blue;
            
            // Print header
            Console.WriteLine("\t\t\tPOPULATION");
            Console.Write("Age");
            for (int i = 0; i < statistics[0].SpeciesNumber; i++)
            {
                Console.Write("\t|{0}", i);
            }

            // Print data
            foreach (Statistic statistic in statistics)
            {
                Console.Write(Environment.NewLine);
                int[] populationInfo = statistic.PopulationInfo;
                Debug.Assert(populationInfo.Length == statistic.SpeciesNumber, "There is no information about some species");
                Console.Write(statistic.Age);
                foreach (int number in populationInfo)
                {
                    Console.Write("\t|{0}", number);
                }
            }

            Console.Write(Environment.NewLine);
        }

        /// <summary>
        /// The print genes info.
        /// </summary>
        /// <param name="statistics">
        /// The statistics.
        /// </param>
        private static void PrintGenesInfo(List<Statistic> statistics)
        {
            Debug.Assert(statistics.Count > 0, "There is no any statistic. WTF!");
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Print header
            Console.Write(Environment.NewLine);
            Console.WriteLine("\t\t\tGENES");
            Console.WriteLine("Age\t|SelfishGene\t|AltruisticGene\t|CreatureLevelGene");

            // Print data
            foreach (Statistic statistic in statistics)
            {
                Console.WriteLine(
                    "{0}\t|{1:f4}\t\t|{2:f4}\t\t|{3:f4}",
                    statistic.Age,
                    statistic.SelfishPercentPerCreature,
                    statistic.AltruisticPercentPerCreature,
                    statistic.CreatureLevelGenesPercentPerCreature);
            }
        }
    }
}
