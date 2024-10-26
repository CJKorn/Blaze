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
    public static List<Report> Reports { get; set; } = new List<Report>();
    public static string debug { get; set; }


	public static List<Location> Locations { get; private set; } = new List<Location>();
}
