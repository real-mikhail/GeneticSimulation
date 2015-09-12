// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Creature.cs" company="">
//   
// </copyright>
// <summary>
//   The person.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeneticSimulation
{
    using System;
    using System.Diagnostics;

    using GeneticSimulation.Genes;
    using GeneticSimulation.Helpers;

    /// <summary>
    /// The person.
    /// </summary>
    public class Creature
    {
        /// <summary>
        /// The id of species.
        /// </summary>
        public readonly int IdOfSpecies;

        /// <summary>
        /// The genes.
        /// </summary>
        private readonly Gene[] genes = new Gene[128];

        /// <summary>
        /// The mother.
        /// </summary>
        private readonly Creature mother;

        /// <summary>
        /// The father.
        /// </summary>
        private readonly Creature father;

        /// <summary>
        /// Initializes a new instance of the <see cref="Creature"/> class.
        /// </summary>
        /// <param name="idOfSpecies">
        /// The id of species.
        /// </param>
        public Creature(int idOfSpecies)
        {
            this.IdOfSpecies = idOfSpecies;
            for (int i = 0; i < this.genes.Length; i++)
            {
                this.genes[i] = EnumHelper.CreateRandomGene();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Creature"/> class.
        /// </summary>
        /// <param name="mommy">
        /// The mommy.
        /// </param>
        /// <param name="daddy">
        /// The daddy.
        /// </param>
        public Creature(Creature mommy, Creature daddy)
        {
            Debug.Assert(mommy.IdOfSpecies == daddy.IdOfSpecies, "Interspecies relation are FORBIDDEN!!!");
            this.mother = mommy;
            this.father = daddy;
            this.IdOfSpecies = mommy.IdOfSpecies;
            for (int i = 0; i < this.genes.Length; i++)
            {
                this.genes[i] = EnumHelper.ChooseRandomGene(mommy.genes[i], daddy.genes[i]);
            }
        }

        /// <summary>
        /// Gets the strength.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int Strength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The mutate.
        /// </summary>
        public void Mutate()
        {
            // Tries to change 6 genes with 50% probability
            int length = this.genes.Length;
            int rnd = RandomProvider.GetThreadRandom().Next(length << 1);
            int limit = Math.Min(length, rnd + 6);
            for (; rnd < limit; rnd++)
            {
                this.genes[rnd] = EnumHelper.CreateRandomGene();
            }
        }
    }
}
