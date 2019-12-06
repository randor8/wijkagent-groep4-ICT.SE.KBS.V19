using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public sealed class FilterList
    {
        private static readonly HashSet<IFilter> _filterSet = new HashSet<IFilter>();

        private static Queue<IFilter> FilterQueue { get; set; } = new Queue<IFilter>();

        static FilterList() { }

        private FilterList() { }

        /// <summary>
        /// Applies all filters in the filterlist to the list of offences recursively.
        /// </summary>
        /// <param name="offences">The list of offences to apply the filter on.</param>
        /// <returns>The list of offences that meet the requirements of the applied filter.</returns>
        public static List<Offence> ApplyFilters(List<Offence> offences)
        {
            if (FilterQueue.Count > 0)
            {
                return ApplyFilters(FilterQueue.Dequeue().ApplyOn(offences));
            } else
            {
                UpdateQueue();
                return offences;
            }
        }

        /// <summary>
        /// Adds a filter to the FilterList.
        /// </summary>
        /// <param name="filter">The filter that needs to be added to the FilterList.</param>
        public static void AddFilter(IFilter filter)
        {
            _filterSet.Add(filter);

            UpdateQueue();
        }
        
        /// <summary>
        /// Removes a filter from the FilterList.
        /// </summary>
        /// <param name="filter">The filter that needs to be removed from the FilterList.</param>
        public static void RemoveFilter(IFilter filter)
        {
            _filterSet.Remove(filter);

            UpdateQueue();
        }

        /// <summary>
        /// Clears the FilterQueue and adds all filters from the filterList to the queue.
        /// </summary>
        private static void UpdateQueue()
        {
            FilterQueue.Clear();
            foreach (IFilter filter in _filterSet)
            {
                FilterQueue.Enqueue(filter);
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

            UpdateQueue();
        }
    }
}
