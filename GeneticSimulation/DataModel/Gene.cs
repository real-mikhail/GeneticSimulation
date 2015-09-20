// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Gene.cs" company="">
//   
// </copyright>
// <summary>
//   The gene.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.DataModel
{
    /// <summary>
    /// The gene.
    /// </summary>
    public enum Gene : byte
    {
        /// <summary>
        /// The selfish gene (affects on all creatures which has high probability of containing this gene - other words - family of creature). Another name - BloodGene.
        /// </summary>
        SelfishGene = 1,

        /// <summary>
        /// The altruistic gene. Helps all species (influence to all population - standard biology model - population is entity of evolution).
        /// </summary>
        AltruisticGene = 2,

        /// <summary>
        /// The creature level gene. Affects only current creature.
        /// </summary>
        CreatureLevelGene = 4
    }
}
