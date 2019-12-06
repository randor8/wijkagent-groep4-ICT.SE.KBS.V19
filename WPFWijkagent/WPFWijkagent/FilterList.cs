using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;

namespace WijkagentWPF
{
    public sealed class FilterList
    {
        private static HashSet<IFilter> instance = new HashSet<IFilter>();

        private static Queue<IFilter> FilterQueue { get; set; } = new Queue<IFilter>();

        static FilterList() { }

        private FilterList() { }

        public static List<Offence> ApplyFilters(List<Offence> offences)
        {
            if (FilterQueue.Count > 0)
            {
                return ApplyFilters(FilterQueue.Dequeue().Filter(offences));
            } else
            {
                UpdateQueue();
                return offences;
            }
        }

        public static void AddFilter(IFilter filter)
        {
            instance.Add(filter);

            UpdateQueue();
        }
        
        public static void RemoveFilter(IFilter filter)
        {
            instance.Remove(filter);

            UpdateQueue();
        }

        private static void UpdateQueue()
        {
            FilterQueue.Clear();
            foreach (IFilter filter in instance)
            {
                FilterQueue.Enqueue(filter);
            }
        }

        public static void ClearFilters()
        {
            instance = new HashSet<IFilter>();

            UpdateQueue();
        }
    }
}
