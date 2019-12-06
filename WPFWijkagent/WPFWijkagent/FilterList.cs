using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public sealed class FilterList
    {
        /// <summary>
        /// HashSet that holds all filters.
        /// </summary>
        private static readonly HashSet<IFilter> _filterSet = new HashSet<IFilter>();

        /// <summary>
        /// Stack used to apply the filters from the HashSet.
        /// </summary>
        private static Stack<IFilter> FilterStack { get; set; } = new Stack<IFilter>();

        static FilterList() { }

        private FilterList() { }

        /// <summary>
        /// Applies all filters in the filterlist to the list of offences recursively.
        /// </summary>
        /// <param name="offences">The list of offences to apply the filter on.</param>
        /// <returns>The list of offences that meet the requirements of the applied filter.</returns>
        public static List<Offence> ApplyFilters(List<Offence> offences)
        {
            if (FilterStack.Count > 0)
            {
                IFilter filter = FilterStack.Pop();
                if (filter.GetType().Equals(typeof(CategoryFilter)))
                {
                    FilterStack.Push(filter);
                    return ApplyCategoryFilter(offences);
                } else
                {
                    return ApplyFilters(filter.ApplyOn(offences));
                }
            } else
            {
                UpdateStack();
                return offences;
            }
        }

        /// <summary>
        /// Applies all CategoryFilters to the given list of offences.
        /// </summary>
        /// <param name="offences">The list of offences that needs to be filtered.</param>
        /// <returns>A list of offences belonging to one of the categories.</returns>
        private static List<Offence> ApplyCategoryFilter(List<Offence> offences)
        {
            List<Offence> filtered = new List<Offence>();
            while (FilterStack.Count > 0)
            {
                CategoryFilter categoryFilter = (CategoryFilter)FilterStack.Pop();
                filtered.AddRange(categoryFilter.ApplyOn(offences));
            }
            filtered.Reverse();
            return filtered;
        }

        /// <summary>
        /// Adds a filter to the FilterList.
        /// </summary>
        /// <param name="filter">The filter that needs to be added to the FilterList.</param>
        public static void AddFilter(IFilter filter)
        {
            _filterSet.Add(filter);

            UpdateStack();
        }
        
        /// <summary>
        /// Removes a filter from the FilterList.
        /// </summary>
        /// <param name="filter">The filter that needs to be removed from the FilterList.</param>
        public static void RemoveFilter(IFilter filter)
        {
            _filterSet.Remove(filter);

            UpdateStack();
        }

        /// <summary>
        /// Clears the FilterQueue and adds all filters from the filterList to the queue.
        /// </summary>
        private static void UpdateStack()
        {
            FilterStack.Clear();
            foreach (IFilter filter in _filterSet)
            {
                FilterStack.Push(filter);
            }
            SortStack();
        }

        /// <summary>
        /// Sorts the filters in the FilterStack with CategoryFilters placed on the bottom of the stack.
        /// </summary>
        private static void SortStack()
        {
            if (FilterStack.Count > 0)
            {
                IFilter filter = FilterStack.Pop();
                SortStack();
                SortedInsert(filter);
            }
        }

        /// <summary>
        /// Recursive helper function for SortStack.
        /// </summary>
        /// <param name="filter"></param>
        private static void SortedInsert(IFilter filter)
        {
            if (FilterStack.Count == 0 || !filter.GetType().Equals(typeof(CategoryFilter)))
            {
                FilterStack.Push(filter);
            } else
            {
                IFilter temp = FilterStack.Pop();
                SortedInsert(filter);
                FilterStack.Push(temp);
            }
        }

        /// <summary>
        /// Returns a list of all IFilter in FilterList.
        /// </summary>
        /// <returns>List of all IFilters.</returns>
        public static List<IFilter> GetFilters()
        {
            List<IFilter> filterList = new List<IFilter>();
            filterList.AddRange(_filterSet);
            return filterList;
        }

        /// <summary>
        /// Removes all filters from the FilterList.
        /// </summary>
        public static void ClearFilters()
        {
            _filterSet.Clear();

            UpdateStack();
        }
    }
}
