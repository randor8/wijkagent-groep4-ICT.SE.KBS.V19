using System;
using System.Collections.Generic;

namespace WijkagentModels
{
    public class Offence
    {
        //returns a list of 4 offence objects
        public static List<Offence> OffenceData()
        {
            return new List<Offence>()
            {
                new Offence(){
                    ID=1,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "een delict..",
                    LocationID= new Location(52.499853,6.081799 )
                },
                new Offence(){
                    ID=2,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "twee delict..",
                    LocationID= new Location(52.498599,6.083880 )
                },
                new Offence() {
                    ID=3,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "drie delict..",
                    LocationID= new Location(52.497853, 6.080799)
                },
                new Offence(){
                    ID=4,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "vier delict..",
                    LocationID= new Location(52.50853,6.084099 )
                }
            };
        }
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public String Category { get; set; }
        public virtual Location LocationID { get; set; }
    }
}
