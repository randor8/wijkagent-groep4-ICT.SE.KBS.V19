﻿using System;
using System.Collections.Generic;

namespace WijkagentModels
{
    public class Offence
    {
        //returns a list of 4 offence objects
        public static List<Offence> OffenceData { get; set; } = new List<Offence>();
         
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public String Category { get; set; }
        public virtual Location LocationID { get; set; }
    }
}
