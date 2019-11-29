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
    }
}
