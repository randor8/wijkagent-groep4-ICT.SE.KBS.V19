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

        public bool Equals([AllowNull] IFilter other)
        {
            if (other is DateFilter that)
            {
                return DateTime.Date.Equals(that.DateTime.Date);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is DateFilter that)
            {
                return DateTime.Date.Equals(that.DateTime.Date);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DateTime);
        }
    }
}
