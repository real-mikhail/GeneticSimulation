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

    /// <summary>
    /// The world.
    /// </summary>
    public class World
    {
        /// <summary>
        /// The species.
        /// </summary>
        public readonly List<Creature>[] Species = new List<Creature>[8];

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        public World()
        {
            for (int i = 0; i < this.Species.Length; i++)
            {
                this.Species[i] = new List<Creature>(65536);
                this.Species[i].AddRange(Enumerable.Range(0, 1024).Select(_ => new Creature(i)));
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
        /// <exception cref="NotImplementedException"></exception>
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
            List<Creature> allCreatures = new List<Creature>(this.Species.Sum(kind => kind.Count));
            allCreatures.AddRange(this.Species.SelectMany(kind => kind));
            allCreatures =
                allCreatures.OrderByDescending(creature => creature.Strength).Take(allCreatures.Count / 2).ToList();
            for (int i = 0; i < this.Species.Length; i++)
            {
                this.Species[i].Clear();
                this.Species[i].AddRange(allCreatures.Where(creature => creature.IdOfSpecies == i));
            }
        }

        /// <summary>
        /// The make children.
        /// </summary>
        private void MakeChildren()
        {
            // 33% of 1 child, 33.5% of 2 and 3 childs.
            // Random parents (of same species) - for supporting different genes
        }

        /// <summary>
        /// The mutate.
        /// </summary>
        private void Mutate()
        {
            
        }
    }
}
