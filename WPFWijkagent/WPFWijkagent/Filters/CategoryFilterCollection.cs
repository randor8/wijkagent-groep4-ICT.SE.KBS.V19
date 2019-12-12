using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF.Filters
{
    public class CategoryFilterCollection : IFilter
    {
        public Dictionary<CategoryFilter, bool> Categories { get; set; }

        public CategoryFilterCollection()
        {
            Categories = new Dictionary<CategoryFilter, bool>();
            foreach(OffenceCategories offenceCategorie in Enum.GetValues(typeof(OffenceCategories))) 
            {
                Categories.Add(new CategoryFilter(offenceCategorie), false);
            }
        }

        public List<Offence> ApplyOn(List<Offence> offences)
        {
            List<Offence> filtered = new List<Offence>();
            foreach(KeyValuePair<CategoryFilter, bool> category in Categories)
            {
                if (category.Value)
                {
                    filtered.AddRange(category.Key.ApplyOn(offences));
                }
            }
            return filtered;
        }
    }
}
