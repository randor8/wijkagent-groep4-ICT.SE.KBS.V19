using System;
using System.Collections.Generic;

namespace WijkagentModels
{
	public class Offence
	{
		//returns a list of 4 offence objects
		public static List<Offence> OffenceData()
		{
			return new List<Offence>
			{
				new Offence{
					ID = 1,
					DateTime = new DateTime().ToLocalTime(),
					Description = "een delict..",
					LocationID = new Location{
						Longitude = 6.081799d,
						Latitude = 52.499853d
					}
				},
				new Offence{
					ID = 2,
					DateTime = new DateTime().ToLocalTime(),
					Description = "twee delict..",
					LocationID = new Location{
						Longitude = 6.083880d,
						Latitude = 52.498599d
					}
				},
				new Offence{
					ID = 3,
					DateTime = new DateTime().ToLocalTime(),
					Description = "drie delict..",
					LocationID = new Location{
						Longitude = 6.080799d,
						Latitude = 52.497853d
					}
				},
				new Offence{
					ID = 4,
					DateTime = new DateTime().ToLocalTime(),
					Description = "vier delict..",
					LocationID = new Location{
						Longitude = 6.084099d,
						Latitude = 52.50853d
					}
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
