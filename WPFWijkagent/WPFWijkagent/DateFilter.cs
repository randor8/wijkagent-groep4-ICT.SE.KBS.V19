using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
        /// Checks whether the given filter filters on the same date.
        /// </summary>
        /// <param name="other">Filter to compare this filter with.</param>
        /// <returns>True if both filters filter on the same date.</returns>
        public bool Equals([AllowNull] IFilter other)
        {
            if (other is DateFilter that)
            {
                return DateTime.Date.Equals(that.DateTime.Date);
            }
            return false;
        }

        /// <summary>
        /// Checks whether the given filter filters on the same date.
        /// </summary>
        /// <param name="other">Filter to compare this filter with.</param>
        /// <returns>True if both filters filter on the same date.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DateFilter that)
            {
                return DateTime.Date.Equals(that.DateTime.Date);
            }
            return false;
        }

        /// <summary>
        /// Generates a hashcode based on the DateTime of this filter.
        /// </summary>
        /// <returns>A hashcode.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(DateTime);
        }
    }
}
