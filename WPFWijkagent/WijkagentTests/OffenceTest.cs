using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentTests
{
    [TestFixture]
    public class OffenceTest
    {
        private static Offence[] offences =
        {
            new Offence(1, new DateTime().ToLocalTime(), "", new Location(0, 1.1, 1.1), OffenceCategories.Null),
            new Offence(2, new DateTime().ToLocalTime(), "", new Location(0, 1.1, 1.1), OffenceCategories.Null),
            new Offence(3, new DateTime().ToLocalTime(), "", new Location(0, 1.1, 1.1), OffenceCategories.Null)
        };

        public static IEnumerable<TestCaseData> EqualsCases
        {
            get
            {
                yield return new TestCaseData(offences[0], offences[1]).Returns(false);
                yield return new TestCaseData(offences[0], offences[2]).Returns(false);
                yield return new TestCaseData(offences[0], offences[0]).Returns(true);
            }
        }

        [TestCaseSource("EqualsCases")]
        public Boolean Equals_TestAllCases(Offence first, Offence second)
        {
            return first.Equals(second);
        }
    }
}
