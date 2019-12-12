using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF.Filters
{
    public class CategoryFilterCollection : IFilter
    {
        private static CategoryFilterCollection instance;

        private CategoryFilterCollection() 
        {
            Categories = new Dictionary<CategoryFilter, bool>();
            foreach (OffenceCategories offenceCategorie in Enum.GetValues(typeof(OffenceCategories)))
            {
                Categories.Add(new CategoryFilter(offenceCategorie), false);
            }
        }

        public static CategoryFilterCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryFilterCollection();
                }
                return instance;
            }
        }

        private static Dictionary<CategoryFilter, bool> Categories { get; set; }

        public void ToggleCategory(OffenceCategories category)
        {
            List<CategoryFilter> filters = new List<CategoryFilter>();
            foreach(CategoryFilter filter in Categories.Keys)
            {
                filters.Add(filter);
            }
            foreach(CategoryFilter filter in filters)
            {
                if (filter.Category.Equals(category))
                {
                    Categories[filter] = !Categories[filter];
                }
            }
        }

        public List<Offence> ApplyOn(List<Offence> offences)
        {
            bool filtered = false;
            List<Offence> filteredList = new List<Offence>();
            foreach(KeyValuePair<CategoryFilter, bool> category in Categories)
            {
                if (category.Value)
                {
                    filteredList.AddRange(category.Key.ApplyOn(offences));
                    filtered = true;
                }
            }
            return filtered ? filteredList : offences;
        }
    }
}
