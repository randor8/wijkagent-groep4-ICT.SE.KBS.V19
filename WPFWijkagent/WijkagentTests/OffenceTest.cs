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
            new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { ID = 1 },
            new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { ID = 2 },
            new Offence(new DateTime().ToLocalTime(), new Location(1.1, 1.1)) { ID = 3 }
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
