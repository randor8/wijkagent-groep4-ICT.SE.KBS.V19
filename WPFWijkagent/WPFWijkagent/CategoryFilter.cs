using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public bool Equals([AllowNull] IFilter other)
        {
            if (other is CategoryFilter that)
            {
                return this.Category.Equals(that.Category);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is CategoryFilter that)
            {
                return this.Category.Equals(that.Category);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Category);
        }
    }
}
