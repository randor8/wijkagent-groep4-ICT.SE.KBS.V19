using System;
using WijkagentModels;
using WijkagentWPF.Filters;

namespace WijkagentWPF.Session
{
    public class SessionFilterCategories : ASession
    {
        public SessionFilterCategories() : base("FilterCategory") { }

        /// <summary>
        /// Checks if the given category exists in the CategoryFilterCOllection and if it has been toggled.
        /// If toggled it returns true.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool IsFilterActive(string category)
        {
            var filters = CategoryFilterCollection.Categories;

            foreach (CategoryFilter filter in filters.Keys)
                if (filter.Category.ToString().Equals(category) && filters[filter])
                    return true;
            return false;
        }

        public override void Load(string input)
        {
            if (input.Length == 0) return;
            var categories = input.Split(Separator);
            foreach (string category in categories)
                CategoryFilterCollection.Instance.ToggleCategory((OffenceCategories)Enum.Parse(typeof(OffenceCategories), category));
        }

        public override string Save()
        {
            var categoryString = "";
            var categoryFilters = CategoryFilterCollection.Categories;

            foreach (var cat in categoryFilters.Keys)
            {
                if (categoryFilters[cat] == false) continue;

                if (categoryString.Length > 0) categoryString += Separator;
                categoryString += cat.Category;
            }

            return categoryString;
        }
    }
}
