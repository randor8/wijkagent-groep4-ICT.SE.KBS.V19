using System;
using System.Collections.Generic;
using System.Linq;
using WijkagentModels;

namespace WijkagentWPF.Filters
{
    public class DateRangeFilter : IFilter
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public DateRangeFilter(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Applies this filter to the given list of offences.
        /// </summary>
        /// <param name="offences">The list of offences that needs to be filtered.</param>
        /// <returns>A list of offences that lie between the dates of the filter</returns>
        public List<Offence> ApplyOn(List<Offence> offences)
        {
            IEnumerable<Offence> filterQuery =
                from offence in offences
                where offence.DateTime.Date >= From.Date && offence.DateTime.Date <= To.Date
                select offence;
            List<Offence> filteredOffences = new List<Offence>();
            filteredOffences.AddRange(filterQuery);
            return filteredOffences;
        }

        /// <summary>
        /// Determines whether the given object is the same as this DateRangeFilter.
        /// </summary>
        /// <param name="obj">The object to compare to this DateRangeFilter</param>
        /// <returns>True if the both the start and end dates are the same, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DateRangeFilter that)
            {
                if (From.Date.Equals(that.From.Date) && To.Date.Equals(that.To.Date))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }
    }
}
