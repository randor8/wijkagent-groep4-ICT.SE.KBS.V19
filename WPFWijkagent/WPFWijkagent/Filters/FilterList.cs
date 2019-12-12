using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentWPF
{
    public sealed class FilterList
    {
        /// <summary>
        /// Dictionary that holds all filters.
        /// </summary>
        private static Dictionary<string, IFilter> _filterSet = new Dictionary<string, IFilter>();

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
                return ApplyFilters(FilterStack.Pop().ApplyOn(offences));
            }
            else
            {
                UpdateStack();
                return offences;
            }
        }

        /// <summary>
        /// Adds a filter to the FilterList.
        /// </summary>
        /// <param name="filter">The filter that needs to be added to the FilterList.</param>
        public static void AddFilter(IFilter filter)
        {
            if (!_filterSet.ContainsKey($"{filter.GetType()}"))
            {
                _filterSet.Add($"{filter.GetType()}", filter);
            }

            UpdateStack();
        }

        /// <summary>
        /// Removes a filter from the FilterList.
        /// </summary>
        /// <param name="filter">The filter that needs to be removed from the FilterList.</param>
        public static void RemoveFilter(IFilter filter)
        {
            _filterSet.Remove($"{filter.GetType()}");

            UpdateStack();
        }

        /// <summary>
        /// Clears the FilterQueue and adds all filters from the filterList to the queue.
        /// </summary>
        private static void UpdateStack()
        {
            FilterStack.Clear();
            foreach (KeyValuePair<string, IFilter> filter in _filterSet)
            {
                FilterStack.Push(filter.Value);
            }
        }

        /// <summary>
        /// Returns a list of all IFilter in FilterList.
        /// </summary>
        /// <returns>List of all IFilters.</returns>
        public static List<IFilter> GetFilters()
        {
            List<IFilter> filterList = new List<IFilter>();
            foreach (KeyValuePair<string, IFilter> filter in _filterSet)
            {
                filterList.Add(filter.Value);
            }
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
