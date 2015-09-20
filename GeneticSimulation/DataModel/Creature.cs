// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Creature.cs" company="">
//   
// </copyright>
// <summary>
//   The person.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MZ.GeneticSimulation.Helpers;
    
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
        private readonly List<Creature> childs = new List<Creature>(8);

        /// <summary>
        /// </summary>
        private readonly World world;

        /// <summary>
        /// The mother.
        /// </summary>
        private Creature mother;

        /// <summary>
        /// The father.
        /// </summary>
        private Creature father;

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
            Contract.Requires<ArgumentNullException>(world != null);
            Contract.Ensures(this.IdOfSpecies == idOfSpecies);
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
        public int SummaryStrength
        {
            get
            {
                double sum = 0.0;
                World world = this.world;
                string cacheKey = $"AltruisticGenesOutStrength{this.IdOfSpecies}";
                object cachedValue = Cache.Get(cacheKey, world.Age);
                if (cachedValue != null)
                {
                    sum = (double)cachedValue;
                }
                else
                {
                    for (int i = 0; i < world.Species[this.IdOfSpecies].Count; i++)
                    {
                        if (world.Species[this.IdOfSpecies][i] != this)
                        {
                            sum += world.Species[this.IdOfSpecies][i].AltruisticGenesOutStrength;
                        }
                    }

                    Cache.Put(cacheKey, world.Age, sum);
                }

                return this.ThisCreatureGenesStrength + (int)sum + (int)this.HelpFromRelations;
            }
        }

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
        {
            get
            {
                int sum = 0;
                for (int i = 0; i < this.genes.Length; i++)
                {
                    Gene gene = this.genes[i];
                    if (gene == Gene.AltruisticGene)
                    {
                        sum += GeneStrength >> 1;
                    }
                }

                return (double)sum / (this.world.Species[this.IdOfSpecies].Count - 1);
            }
        }

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

            // TODO: Potentially incorrect - only first half of genes is mutated
            for (; rnd < limit; rnd++)
            {
                this.genes[rnd] = EnumHelper.CreateRandomGene();
            }
        }

        /// <summary>
        /// </summary>
        public void BreakRedundantConnections()
        {
            Creature mommy = this.mother;
            Creature daddy = this.father;

            // TODO: I suppose it is a .NET bug (we should not clear list of childs?) Fix it and send to github/dotnet
            if (mommy?.mother?.mother != null)
            {
                mommy.mother.mother?.childs.Clear();
                mommy.mother.mother = null;
                mommy.mother.father?.childs.Clear();
                mommy.mother.father = null;
                mommy.father.mother?.childs.Clear();
                mommy.father.mother = null;
                mommy.father.father?.childs.Clear();
                mommy.father.father = null;
                daddy.mother.mother?.childs.Clear();
                daddy.mother.mother = null;
                daddy.mother.father?.childs.Clear();
                daddy.mother.father = null;
                daddy.father.mother?.childs.Clear();
                daddy.father.mother = null;
                daddy.father.father?.childs.Clear();
                daddy.father.father = null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="whoAreYou">
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
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
