﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WijkagentModels
{
    public class Location
    {
        public int ID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Location( double longtitude, double latitude)
        {
            Longitude = longtitude;
            Latitude = latitude;
        }
    }
}
