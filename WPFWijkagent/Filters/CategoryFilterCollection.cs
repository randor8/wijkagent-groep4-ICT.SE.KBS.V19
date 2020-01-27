using System;
using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentWPF.Filters
{
    public class CategoryFilterCollection : IFilter
    {
        private static CategoryFilterCollection instance;

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

        private CategoryFilterCollection()
        {
            Categories = new Dictionary<CategoryFilter, bool>();
            foreach (OffenceCategories offenceCategorie in Enum.GetValues(typeof(OffenceCategories)))
            {
                Categories.Add(new CategoryFilter(offenceCategorie), false);
            }
        }

        /// <summary>
        /// Dictionary containing CategoryFilters as key and whether they are turn on as value.
        /// </summary>
        public static Dictionary<CategoryFilter, bool> Categories { get; private set; }

        /// <summary>
        /// Toggles a CategoryFilter on or off. 
        /// </summary>
        /// <param name="category">The category of the CategoryFilter to toggle.</param>
        public void ToggleCategory(OffenceCategories category)
        {
            List<CategoryFilter> filters = new List<CategoryFilter>();
            foreach (CategoryFilter filter in Categories.Keys)
            {
                filters.Add(filter);
            }
            foreach (CategoryFilter filter in filters)
            {
                if (filter.Category.Equals(category))
                {
                    Categories[filter] = !Categories[filter];
                }
            }
        }

        /// <summary>
        /// Turns all filters off.
        /// </summary>
        public void ShowAll()
        {
            List<CategoryFilter> filters = new List<CategoryFilter>();
            foreach (CategoryFilter filter in Categories.Keys)
            {
                filters.Add(filter);
            }
            foreach (CategoryFilter filter in filters)
            {
                Categories[filter] = false;
            }
        }

        /// <summary>
        /// Applies all CategoryFilters in the collection to the given list of offences.
        /// </summary>
        /// <param name="offences">The list of offences to filter.</param>
        /// <returns>A list of offences meeting the requirements set by the filters.</returns>
        public List<Offence> ApplyOn(List<Offence> offences)
        {
            bool filtered = false;
            List<Offence> filteredList = new List<Offence>();
            foreach (KeyValuePair<CategoryFilter, bool> category in Categories)
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
