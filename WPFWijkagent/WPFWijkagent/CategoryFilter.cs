using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<Offence> Filter(List<Offence> offences)
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
