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
                    Category = OffenceCategories.categorie1,
                    LocationID= new Location(){ 
                        Longitude=52.499853m,
                        Latitude=6.081799m
                    },
                },
                new Offence(){
                    ID=2,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "twee delict..",
                    Category = OffenceCategories.categorie2,
                    LocationID= new Location(){
                        Longitude=52.498599m,
                        Latitude=6.083880m
                    },
                },
                new Offence() {
                    ID=3,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "drie delict..",
                    Category = OffenceCategories.categorie3,
                    LocationID= new Location(){
                        Longitude=52.497853m,
                        Latitude=6.080799m
                    },
                },
                new Offence(){
                    ID=4,
                    DateTime= new DateTime().ToLocalTime(),
                    Description = "vier delict..",
                    Category = OffenceCategories.categorie4,
                    LocationID= new Location(){
                        Longitude=52.50853m,
                        Latitude=6.084099m
                    },
                }
            };
        }
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public OffenceCategories Category { get; set; }
        public virtual Location LocationID { get; set; }
    }
}
