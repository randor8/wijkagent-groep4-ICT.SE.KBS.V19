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
    }
}
