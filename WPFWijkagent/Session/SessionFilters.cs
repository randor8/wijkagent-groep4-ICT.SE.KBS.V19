using System;
using System.Windows.Controls;
using WijkagentModels;
using WijkagentWPF.Filters;

namespace WijkagentWPF.Session
{
    public class SessionActiveDateFilter : ASession
    {
        protected readonly RadioButton _single;
        protected readonly RadioButton _range;

        public SessionActiveDateFilter(RadioButton single, RadioButton range) : base("ActiveDateFilter")
        {
            _single = single;
            _range = range;
        }

        public override void Load(string input)
        {
            if (input.Length == 0) return;
            if (input.Equals("Single"))
            {
                _single.IsChecked = true;
            }
            else if (input.Equals("Range"))
            {
                _range.IsChecked = true;
            }
        }

        public override string Save()
        {
            string dateString = "";
            if (_single.IsChecked.Value && !_range.IsChecked.Value)
            {
                dateString += "Single";
            }
            else if (!_single.IsChecked.Value && _range.IsChecked.Value)
            {
                dateString += "Range";
            }
            return dateString;
        }
    }

    public class SessionFilterDateRange : ASession
    {
        protected readonly DatePicker _datePickerFrom;
        protected readonly DatePicker _datePickerTo;

        public SessionFilterDateRange(DatePicker from, DatePicker to) : base("FilterDateRange")
        {
            _datePickerFrom = from;
            _datePickerTo = to;
        }

        public override void Load(string input)
        {
            if (input.Length == 0) return;
            var dates = input.Split(Separator);
            DateTime from = new DateTime(int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]));
            _datePickerFrom.SelectedDate = from;
            DateTime to = new DateTime(int.Parse(dates[3]), int.Parse(dates[4]), int.Parse(dates[5]));
            _datePickerTo.SelectedDate = to;
        }

        public override string Save()
        {
            string dateString = "";
            if (_datePickerFrom.SelectedDate != null && _datePickerTo.SelectedDate != null)
            {
                DateTime from = _datePickerFrom.SelectedDate.Value;
                DateTime to = _datePickerTo.SelectedDate.Value;
                dateString += from.Year.ToString() + Separator + from.Month.ToString() + Separator + from.Day.ToString() + Separator;
                dateString += to.Year.ToString() + Separator + to.Month.ToString() + Separator + to.Day.ToString();
            }
            return dateString;
        }
    }

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
            if (_datePicker.SelectedDate != null)
            {
                DateTime dateTime = _datePicker.SelectedDate.Value;
                dateString += dateTime.Year.ToString() + Separator + dateTime.Month.ToString() + Separator + dateTime.Day.ToString();
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
