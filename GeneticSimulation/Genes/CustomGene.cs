// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomGene.cs" company="">
//   
// </copyright>
// <summary>
//   The custom gene.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GeneticSimulation.Genes
{
    using System.Net.NetworkInformation;

    /// <summary>
    /// The custom gene.
    /// </summary>
    public class CustomGene : Gene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGene"/> class. 
        /// The custom gene.
        /// </summary>
        /// <exception cref="NetworkInformationException">
        /// Condition.
        /// </exception>
        public CustomGene()
        {
            throw new NetworkInformationException();
        }
    }
}
