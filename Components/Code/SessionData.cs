using System.Text.Json;
using System.IO;

internal static class SessionData
{
    public static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static string jsonFilePath = Path.Combine(path, "Blaze/data.json");
    public static string apiFilePath = Path.Combine(path, "Blaze/keys.apikeys");

    public static bool signedIn { get; set; }
    public static bool hosting { get; set; }
    public static string myIP { get; set; }
    public static string IP { get; set; }
    public static string Key { get; set; }
    public static double Lat { get; set; }
    public static double Lng { get; set; }
    public static int Zoom { get; set; }
    public static double ClickedLat { get; set; }
    public static double ClickedLng { get; set; }
    public static bool Clicked { get; set; }
    public static List<Report> Reports = new List<Report>();

 
    public static List<Location> Locations { get; private set; } = new List<Location>();

 
    public static void LoadLocations()
    {
        if (!File.Exists(jsonFilePath))
            throw new FileNotFoundException("JSON file not found at path: " + jsonFilePath);

        string jsonData = File.ReadAllText(jsonFilePath);
        Locations = JsonSerializer.Deserialize<List<Location>>(jsonData) ?? new List<Location>();
    }

    public static void SaveLocation(double lat, double lng)
    {
        var newLocation = new Location { Lat = lat, Lng = lng };

        // Load existing locations if any
        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            var locations = JsonSerializer.Deserialize<List<Location>>(jsonData) ?? new List<Location>();
            locations.Add(newLocation);

            // Save back to file
            File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(locations));
        }
        else
        {
            // Create new file if none exists
            var locations = new List<Location> { newLocation };
            File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(locations));
        }
    }


    // This is for the markers specifiically. If I'm not mistaken the lat and lng above is for the map to load in the right place?
    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
