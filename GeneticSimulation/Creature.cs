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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using GeneticSimulation.Genes;
    using GeneticSimulation.Helpers;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// The person.
    /// </summary>
    public class Creature
    {
        /// <summary>
        /// </summary>
        private const int GeneStrength = 128;

        /// <summary>
        /// The id of species.
        /// </summary>
        public readonly int IdOfSpecies;

        /// <summary>
        /// The genes.
        /// </summary>
        private readonly Gene[] genes = new Gene[128];

        /// <summary>
        /// </summary>
        private readonly List<Creature> childs = new List<Creature>(4);

        /// <summary>
        /// </summary>
        private readonly World world;

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
        /// <param name="world">
        /// The world.
        /// </param>
        public Creature(int idOfSpecies, World world)
        {
            Requires(world != null);
            Ensures(this.IdOfSpecies == idOfSpecies);
            this.IdOfSpecies = idOfSpecies;
            this.world = world;
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
            mommy.childs.Add(this);
            daddy.childs.Add(this);
            this.world = mommy.world;
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
        public int SummaryStrength
            =>
                this.ThisCreatureGenesStrength
                + (int)this.world.Species[this.IdOfSpecies].Sum(creature => creature.AltruisticGenesOutStrength)
                + (int)this.HelpFromRelations;

        /// <summary>
        /// </summary>
        public int SelfishGenes => this.genes.Count(g => g == Gene.SelfishGene);

        /// <summary>
        /// </summary>
        public int AltruisticGenes => this.genes.Count(g => g == Gene.AltruisticGene);

        /// <summary>
        /// </summary>
        public int CreatureLevelGenes => this.genes.Count(g => g == Gene.CreatureLevelGene);

        /// <summary>
        /// </summary>
        private int ThisCreatureGenesStrength
            => this.genes.Sum(g => g == Gene.CreatureLevelGene ? GeneStrength : GeneStrength >> 1);

        /// <summary>
        /// </summary>
        private double AltruisticGenesOutStrength
            =>
                this.genes.Sum(g => g == Gene.AltruisticGene ? GeneStrength >> 1 : 0)
                / (this.world.Species[this.IdOfSpecies].Count - 1);

        /// <summary>
        /// </summary>
        private double HelpFromRelations
        {
            get
            {
                Creature mommy = this.mother;
                Creature daddy = this.father;
                if (mommy == null)
                {
                    return 0;
                }

                if (mommy.mother == null)
                {
                    return mommy.GetSelfishGenesOutStrength(Relation.Child)
                           + daddy.GetSelfishGenesOutStrength(Relation.Child)
                           + mommy.childs.Sum(
                               brother =>
                               brother == this ? 0 : brother.GetSelfishGenesOutStrength(Relation.BrotherOrSister));
                }

                return mommy.GetSelfishGenesOutStrength(Relation.Child)
                       + daddy.GetSelfishGenesOutStrength(Relation.Child)
                       + mommy.childs.Sum(
                           brother => brother == this ? 0 : brother.GetSelfishGenesOutStrength(Relation.BrotherOrSister))
                       + mommy.mother.GetSelfishGenesOutStrength(Relation.GrandChild)
                       + mommy.father.GetSelfishGenesOutStrength(Relation.GrandChild)
                       + daddy.mother.GetSelfishGenesOutStrength(Relation.GrandChild)
                       + daddy.father.GetSelfishGenesOutStrength(Relation.GrandChild)
                       + mommy.mother.childs.Sum(
                           aunt => aunt == mommy ? 0 : aunt.GetSelfishGenesOutStrength(Relation.NephewOrNiece))
                       + daddy.mother.childs.Sum(
                           uncle => uncle == daddy ? 0 : uncle.GetSelfishGenesOutStrength(Relation.NephewOrNiece))
                       + mommy.mother.childs.Sum(
                           aunt =>
                           aunt == mommy
                               ? 0
                               : aunt.childs.Sum(cousin => cousin.GetSelfishGenesOutStrength(Relation.Cousin)))
                       + daddy.mother.childs.Sum(
                           uncle =>
                           uncle == daddy
                               ? 0
                               : uncle.childs.Sum(cousin => cousin.GetSelfishGenesOutStrength(Relation.Cousin)));
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

        /// <summary>
        /// </summary>
        /// <param name="whoAreYou"></param>
        /// <returns></returns>
        private double GetSelfishGenesOutStrength(Relation whoAreYou)
        {
            Creature mommy = this.mother;
            Creature daddy = this.father;
            int summarySelfishStrength = this.genes.Sum(g => g == Gene.SelfishGene ? GeneStrength >> 1 : 0);
            switch (whoAreYou)
            {
                case Relation.Child:
                    return summarySelfishStrength / this.childs.Count * 30.78;
                case Relation.BrotherOrSister:
                    Debug.Assert(mommy.childs.Count > 1, "LIER! He is not our brother!");
                    return summarySelfishStrength / (mommy.childs.Count - 1) * 30.78;
                case Relation.GrandChild:
                    return summarySelfishStrength / this.childs.Sum(creature => creature.childs.Count) * 15.38;
                case Relation.NephewOrNiece:
                    Debug.Assert(mommy.childs.Count > 1, "LIER! We don't have any brothers!");
                    return summarySelfishStrength
                           / mommy.childs.Sum(brother => brother == this ? 0 : brother.childs.Count) * 15.38;
                case Relation.Cousin:
                    return summarySelfishStrength
                           / (mommy.mother.childs.Sum(aunt => aunt == mommy ? 0 : aunt.childs.Count)
                              + daddy.mother.childs.Sum(uncle => uncle == daddy ? 0 : uncle.childs.Count)) * 7.68;
                default:
                    throw new NotImplementedException("Unknown enum value");
            }
        }
    }
}
