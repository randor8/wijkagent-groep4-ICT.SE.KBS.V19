namespace WijkagentModels
{
    public class Location
    {
        public int ID { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public Location(double latitude, double longtitude)
        {
            Longitude = longtitude;
            Latitude = latitude;

        }

        /// <summary>
        /// Compares given object with this object.
        /// </summary>
        /// <param name="obj">Object to compare this object with.</param>
        /// <returns>True if given object and this object are the same, false otherwise.</returns>      
        public override bool Equals(object obj)
        {
            if (obj is Location)
            {
                Location that = (Location)obj;
                if (this.Latitude == that.Latitude && this.Longitude == that.Longitude)
                {
                    return true;
                } 
            }
            return false;
        }
    }
}
