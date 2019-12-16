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

        public override bool Equals(object obj)
        {
            if (obj is DateRangeFilter that)
            {
                if (From.Date.Equals(that.From.Date) && To.Date.Equals(that.From.Date))
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
