// --------------------------------------------------------------------------------------------------------------------
// <copyright file="World.cs" company="">
//   
// </copyright>
// <summary>
//   The world.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeneticSimulation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GeneticSimulation.Helpers;

    /// <summary>
    /// The world.
    /// </summary>
    public class World
    {
        /// <summary>
        /// The species.
        /// </summary>
        private readonly List<Creature>[] species = new List<Creature>[8];

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        public World()
        {
            for (int i = 0; i < this.species.Length; i++)
            {
                this.species[i] = new List<Creature>(65536);
                this.species[i].AddRange(Enumerable.Range(0, 1024).Select(_ => new Creature(i)));
            }
        }

        /// <summary>
        /// Gets the statistic.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public Statistic Statistic
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="generations"></param>
        public void Run(int generations)
        {
            for (int i = 0; i < generations; i++)
            {
                this.SelectBest();
                this.MakeChildren();
                this.Mutate();
            }
        }

        /// <summary>
        /// The select best.
        /// </summary>
        private void SelectBest()
        {
            var allCreatures = new List<Creature>(this.species.Sum(kind => kind.Count));
            allCreatures.AddRange(this.species.SelectMany(kind => kind));
            allCreatures =
                allCreatures.OrderByDescending(creature => creature.Strength).Take(allCreatures.Count / 2).ToList();
            for (int i = 0; i < this.species.Length; i++)
            {
                this.species[i].Clear();
                this.species[i].AddRange(allCreatures.Where(creature => creature.IdOfSpecies == i));
            }
        }

        /// <summary>
        /// The make children.
        /// </summary>
        private void MakeChildren()
        {
            Parallel.For(
                0,
                this.species.Length,
                i =>
                    {
                        var temp = new List<Creature>(this.species[i].Count << 1);

                        // Random parents (of same species) - for supporting different genes
                        this.species[i].Shuffle();
                        Random rnd = RandomProvider.GetThreadRandom();
                        for (int j = 1; j < this.species[i].Count; j++)
                        {
                            double value = rnd.NextDouble();
                            if (value < 0.33)
                            {
                                temp.Add(new Creature(this.species[i][j - 1], this.species[i][j]));
                            }
                            else if (value < 0.665)
                            {
                                temp.Add(new Creature(this.species[i][j - 1], this.species[i][j]));
                                temp.Add(new Creature(this.species[i][j - 1], this.species[i][j]));
                            }
                            else
                            {
                                temp.Add(new Creature(this.species[i][j - 1], this.species[i][j]));
                                temp.Add(new Creature(this.species[i][j - 1], this.species[i][j]));
                                temp.Add(new Creature(this.species[i][j - 1], this.species[i][j]));
                            }
                        }

                        this.species[i].Clear();
                        this.species[i] = temp;
                    });
        }

        /// <summary>
        /// The mutate.
        /// </summary>
        private void Mutate()
        {
            Parallel.ForEach(this.species, list => list.ForEach(creature => creature.Mutate()));
        }
    }
}
