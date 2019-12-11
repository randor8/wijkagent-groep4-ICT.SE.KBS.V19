using NUnit.Framework;
using System;
using System.Collections.Generic;
using WijkagentModels;

namespace WijkagentTests
{
    [TestFixture]
    public class LocationTest
    {
        private static Location[] Locations =
        {
            new Location(0, 1.1, 1.1),
            new Location(0, 1.1, 1.1),
            new Location(0, 1.1, 1.2),
            new Location(0, 1.2, 1.1),
            new Location(0, 1.2, 1.2)
        };

        public static IEnumerable<TestCaseData> EqualsCases
        {
            get
            {
                yield return new TestCaseData(Locations[0], Locations[1]).Returns(true);
                yield return new TestCaseData(Locations[0], Locations[2]).Returns(false);
                yield return new TestCaseData(Locations[2], Locations[3]).Returns(false);
                yield return new TestCaseData(Locations[1], Locations[4]).Returns(false);
                yield return new TestCaseData(Locations[2], Locations[4]).Returns(false);
            }
        }

        [TestCaseSource("EqualsCases")]
        public Boolean Equals_TestAllCases(Location first, Location second)
        {
            return first.Equals(second);
        }
    }
}
