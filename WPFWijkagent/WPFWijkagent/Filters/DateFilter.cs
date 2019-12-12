using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WijkagentModels;

namespace WijkagentWPF
{
    public class DateFilter : IFilter
    {
        public DateTime DateTime { get; }

        public DateFilter(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        /// <summary>
        /// Applies this filter to the given list of offences.
        /// </summary>
        /// <param name="offences">The list of offences that needs to be filtered.</param>
        /// <returns>A list of offences that have the same date as the filter</returns>
        public List<Offence> ApplyOn(List<Offence> offences)
        {
            IEnumerable<Offence> filterQuery =
                from offence in offences
                where offence.DateTime.Date.Equals(DateTime.Date)
                select offence;
            List<Offence> filteredOffences = new List<Offence>();
            filteredOffences.AddRange(filterQuery);
            return filteredOffences;
        }

        /// <summary>
        /// Determines whether the given object is the same as this DateFilter.
        /// </summary>
        /// <param name="obj">The object to compare to this DateFilter</param>
        /// <returns>True if the dates are the same, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DateFilter that)
            {
                return DateTime.Date.Equals(that.DateTime.Date);
            }
            return false;
        }

        /// <summary>
        /// Calculates a hashcode based on the DateTime.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(DateTime);
        }
    }
}
