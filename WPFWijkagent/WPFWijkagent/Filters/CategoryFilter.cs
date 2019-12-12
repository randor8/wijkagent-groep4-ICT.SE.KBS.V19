using System.Collections.Generic;
using System.Linq;
using WijkagentModels;

namespace WijkagentWPF
{
    public class CategoryFilter
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
    }
}
