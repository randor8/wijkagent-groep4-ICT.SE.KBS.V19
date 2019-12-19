using System;
using System.Windows.Controls;
using WijkagentModels;
using WijkagentWPF.Filters;

namespace WijkagentWPF.Session
{
    public class SessionFilterSingleDate : ASession
    {
        protected readonly DatePicker _datePicker;
        public SessionFilterSingleDate(DatePicker datePicker) : base("FilterSingleDate")
        {
            _datePicker = datePicker;
        }

        public override void Load(string input)
        {
            if (input.Length == 0) return;
            var date = input.Split(Separator);
            DateTime dateTime = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]));
            _datePicker.SelectedDate = dateTime;
        }

        public override string Save()
        {
            string dateString = "";
            DateFilter filter = (DateFilter)FilterList.GetFilters().Find(_ => _ is DateFilter);
            if (filter != null)
            {
                dateString += filter.DateTime.Date.ToString();
            }
            return dateString;
        }
    }

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
