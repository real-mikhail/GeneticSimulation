// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Statistic.cs" company="">
//   
// </copyright>
// <summary>
//   The statistic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeneticSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The statistic.
    /// </summary>
    public class Statistic
    {
        /// <summary>
        /// The species number.
        /// </summary>
        public readonly int SpeciesNumber;

        /// <summary>
        /// The age.
        /// </summary>
        public readonly int Age;

        /// <summary>
        /// Initializes a new instance of the <see cref="Statistic"/> class.
        /// </summary>
        /// <param name="age">
        /// The age.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        public Statistic(int age, World world)
        {
            Contract.Requires<ArgumentNullException>(world != null);
            this.Age = age;
            this.SpeciesNumber = world.Species.Length;
            this.PopulationInfo = new int[this.SpeciesNumber];
            int selfishGenes = 0;
            int altruisticGenes = 0;
            int creatureLevelGenes = 0;
            Parallel.For(
                0,
                world.Species.Length,
                i =>
                    {
                        int threadSelfishGenes = 0;
                        int threadAltruisticGenes = 0;
                        int threadCreatureLevelGenes = 0;
                        List<Creature> creatures = world.Species[i];
                        this.PopulationInfo[i] = creatures.Count;
                        checked
                        {
                            foreach (Creature creature in creatures)
                            {
                                threadSelfishGenes += creature.SelfishGenes;
                                threadAltruisticGenes += creature.AltruisticGenes;
                                threadCreatureLevelGenes += creature.CreatureLevelGenes;
                            }

                            Interlocked.Add(ref selfishGenes, threadSelfishGenes);
                            Interlocked.Add(ref altruisticGenes, threadAltruisticGenes);
                            Interlocked.Add(ref creatureLevelGenes, threadCreatureLevelGenes);
                        }
                    });

            int allGenes;
            checked
            {
                allGenes = selfishGenes + altruisticGenes + creatureLevelGenes;
            }

            this.SelfishPercentPerCreature = (double)selfishGenes / allGenes;
            this.AltruisticPercentPerCreature = (double)altruisticGenes / allGenes;
            this.CreatureLevelGenesPercentPerCreature = (double)creatureLevelGenes / allGenes;
        }

        /// <summary>
        /// Gets the population info.
        /// </summary>
        public int[] PopulationInfo { get; }

        /// <summary>
        /// Gets the selfish percent per creature.
        /// </summary>
        public double SelfishPercentPerCreature { get; }

        /// <summary>
        /// Gets the altruistic percent per creature.
        /// </summary>
        public double AltruisticPercentPerCreature { get; }

        /// <summary>
        /// Gets the creature level genes percent per creature.
        /// </summary>
        public double CreatureLevelGenesPercentPerCreature { get; }
    }
}
