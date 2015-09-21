// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Relation.cs" company="MZ">
//   This work is licensed under a Creative Commons Attribution 4.0 International License
// </copyright>
// <summary>
//   Defines the Relation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MZ.GeneticSimulation.DataModel
{
    /// <summary>
    ///     The relation.
    /// </summary>
    public enum Relation
    {
        /// <summary>
        ///     The child.
        /// </summary>
        Child,

        /// <summary>
        ///     The brother or sister.
        /// </summary>
        BrotherOrSister,

        /// <summary>
        ///     The grand child.
        /// </summary>
        GrandChild,

        /// <summary>
        ///     The nephew or niece.
        /// </summary>
        NephewOrNiece,

        /// <summary>
        ///     The cousin.
        /// </summary>
        Cousin
    }
}