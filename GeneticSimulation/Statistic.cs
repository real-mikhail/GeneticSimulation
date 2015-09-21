// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Statistic.cs" company="">
//   
// </copyright>
// <summary>
//   The statistic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using MZ.GeneticSimulation.DataModel;

    /// <summary>
    ///     The statistic.
    /// </summary>
    public class Statistic
    {
        /// <summary>
        ///     The age.
        /// </summary>
        public readonly int Age;

        /// <summary>
        ///     The species number.
        /// </summary>
        public readonly int SpeciesNumber;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Statistic" /> class.
        /// </summary>
        /// <param name="age">
        ///     The age.
        /// </param>
        /// <param name="world">
        ///     The world.
        /// </param>
        public Statistic(int age, World world)
        {
            Debug.Assert(world != null);
            this.Age = age;
            this.SpeciesNumber = world.Species.Length;
            this.PopulationInfo = new int[this.SpeciesNumber];
            var selfishGenes = 0;
            var altruisticGenes = 0;
            var creatureLevelGenes = 0;
            Parallel.For(
                0,
                world.Species.Length,
                i =>
                    {
                        var threadSelfishGenes = 0;
                        var threadAltruisticGenes = 0;
                        var threadCreatureLevelGenes = 0;
                        var creatures = world.Species[i];
                        this.PopulationInfo[i] = creatures.Count;
                        checked
                        {
                            foreach (var creature in creatures)
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
        ///     Gets the population info.
        /// </summary>
        public int[] PopulationInfo { get; }

        /// <summary>
        ///     Gets the selfish percent per creature.
        /// </summary>
        public double SelfishPercentPerCreature { get; }

        /// <summary>
        ///     Gets the altruistic percent per creature.
        /// </summary>
        public double AltruisticPercentPerCreature { get; }

        /// <summary>
        ///     Gets the creature level genes percent per creature.
        /// </summary>
        public double CreatureLevelGenesPercentPerCreature { get; }
    }
}