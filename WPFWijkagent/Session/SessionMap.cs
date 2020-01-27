using Microsoft.Maps.MapControl.WPF;

namespace WijkagentWPF.Session
{
    public abstract class ASessionMap : ASession
    {
        protected readonly Map _map;

        public ASessionMap(string key, Map map) : base(key) => _map = map;
    }

    public class SessionMapLocation : ASessionMap
    {
        public SessionMapLocation(Map map) : base("MapCenter", map) { }

        public override void Load(string input)
        {
            var parts = input.Split(Separator, 2); // Splitting value into longitude and latitude
            _map.Center = new Location
            {
                Longitude = double.Parse(parts[0]),
                Latitude = double.Parse(parts[1])
            };
        }

        public override string Save() => $"{_map.Center.Longitude}{Separator}{_map.Center.Latitude}";
    }

    public class SessionMapZoom : ASessionMap
    {
        public SessionMapZoom(Map map) : base("MapZoom", map) { }

        public override void Load(string input) => _map.ZoomLevel = double.Parse(input);

        public override string Save() => $"{_map.ZoomLevel}";
    }
}
