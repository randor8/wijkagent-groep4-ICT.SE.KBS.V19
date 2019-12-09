using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WijkagentModels;

namespace WijkagentWPF
{
    public class CategoryFilter : IFilter
    {
        public OffenceCategories Category { get; }

        public CategoryFilter(OffenceCategories category)
        {
            Category = category;
        }

        /// <summary>
        /// Applies this filter to the given list of offences.
        /// </summary>
        /// <param name="offences">The list of offences that needs to be filtered.</param>
        /// <returns>A list of offences that have the same category as the filter.</returns>
        public List<Offence> ApplyOn(List<Offence> offences)
        {
            IEnumerable<Offence> filterQuery =
                from offence in offences
                where offence.Category.Equals(Category)
                select offence;
            List<Offence> filteredOffences = new List<Offence>();
            filteredOffences.AddRange(filterQuery);
            return filteredOffences;
        }

        /// <summary>
        /// Checks whether the given filter filters on the same category.
        /// </summary>
        /// <param name="other">Filter to compare this filter with.</param>
        /// <returns>True if both filters filter on the same category.</returns>
        public bool Equals([AllowNull] IFilter other)
        {
            if (other is CategoryFilter that)
            {
                return Category.Equals(that.Category);
            }
            return false;
        }

        /// <summary>
        /// Checks whether the given filter filters on the same category.
        /// </summary>
        /// <param name="other">Filter to compare this filter with.</param>
        /// <returns>True if both filters filter on the same category.</returns>
        public override bool Equals(object obj)
        {
            if (obj is CategoryFilter that)
            {
                return Category.Equals(that.Category);
            }
            return false;
        }

        /// <summary>
        /// Generates a hashcode based on the category of this filter.
        /// </summary>
        /// <returns>A hashcode.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Category);
        }
    }
}
