namespace WijkagentModels
{
    public class Location
    {
        public int ID { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public Location(int id, double latitude, double longtitude)
        {
            ID = id;
            Longitude = longtitude;
            Latitude = latitude;
        }
    }
}
