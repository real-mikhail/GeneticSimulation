// --------------------------------------------------------------------------------------------------------------------
// <copyright file="World.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
// </copyright>
// <summary>
//   The world.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.DataModel
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using MZ.GeneticSimulation.Helpers;

    /// <summary>
    ///     The world.
    /// </summary>
    public class World
    {
        /// <summary>
        ///     The species.
        /// </summary>
        public readonly List<Creature>[] Species = new List<Creature>[8];

        /// <summary>
        ///     The age of this world.
        /// </summary>
        private int age = 1;

        /// <summary>
        ///     Initializes a new instance of the <see cref="World" /> class.
        /// </summary>
        public World()
        {
            for (var i = 0; i < this.Species.Length; i++)
            {
                this.Species[i] = new List<Creature>(65536);
                this.Species[i].AddRange(Enumerable.Range(0, 1024).Select(_ => new Creature(i, this)));
            }
        }

        /// <summary>
        ///     Gets the statistic.
        /// </summary>
        public Statistic Statistic => new Statistic(this.Age, this);

        /// <summary>
        ///     Gets the age.
        /// </summary>
        public int Age => this.age;

        /// <summary>
        ///     Simulates evolution.
        /// </summary>
        /// <param name="generations">
        ///     Number of iterations of simulation.
        /// </param>
        public void Run(int generations)
        {
            for (var i = 0; i < generations; i++, this.age = this.Age + 1)
            {
                this.SelectBest();
                this.MakeChildren();
                this.Mutate();
                Debug.Print("Age: {0}", i);
            }
        }

        /// <summary>
        ///     Selects best creatures.
        /// </summary>
        private void SelectBest()
        {
            var allCreatures = new List<Creature>(this.Species.Sum(kind => kind.Count));
            allCreatures.AddRange(this.Species.SelectMany(kind => kind));
            allCreatures =
                allCreatures.OrderByDescending(creature => creature.SummaryStrength)
                    .Take(allCreatures.Count >> 1)
                    .ToList();
            for (var i = 0; i < this.Species.Length; i++)
            {
                this.Species[i].ForEach(creature => creature.BreakRedundantConnections());
                this.Species[i].Clear();
                this.Species[i].AddRange(allCreatures.Where(creature => creature.IdOfSpecies == i));
            }
        }

        /// <summary>
        ///     Creates new generation of creatures.
        /// </summary>
        private void MakeChildren()
        {
            Parallel.For(
                0,
                this.Species.Length,
                i =>
                    {
                        var temp = new List<Creature>(this.Species[i].Count << 1);

                        // Random parents (of same species) - for supporting different genes
                        this.Species[i].Shuffle();
                        var rnd = RandomProvider.GetThreadRandom();
                        for (var j = 1; j < this.Species[i].Count; j += 2)
                        {
                            var value = rnd.NextDouble();
                            if (value < 0.33)
                            {
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                            }
                            else if (value < 0.665)
                            {
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                            }
                            else
                            {
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                                temp.Add(new Creature(this.Species[i][j - 1], this.Species[i][j]));
                            }
                        }

                        this.Species[i].ForEach(creature => creature.BreakRedundantConnections());
                        this.Species[i].Clear();
                        this.Species[i] = temp;
                    });
        }

        /// <summary>
        ///     Mutates current generation of creatures.
        /// </summary>
        private void Mutate()
        {
            Parallel.ForEach(this.Species, list => list.ForEach(creature => creature.Mutate()));
        }
    }
}