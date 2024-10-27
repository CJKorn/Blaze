using System.Text.Json;
using System.IO;
using GoogleMapsComponents.Maps;
//this class is used to declare various data elements that can be read and modified from different locations, and it also has the file paths for the .JSON 
//and the API file.
internal static class SessionData
{
    //below code declares static string data elements that correspond with the file path for the .JSON file and the API file.
    public static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static string jsonFilePath = Path.Combine(path, "Blaze/data.json");
    public static string apiFilePath = Path.Combine(path, "Blaze/keys.apikeys");
    //below code declares various data elements and the getters and setters for them
    //by stating get; set; it means that the getters and setters are default, and the elements can be read and modified from anywhere that can access SessionData
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
    public static Report SelectedReport { get; set; }
    public static Marker SelectedMarker { get; set; }



    public static List<Location> Locations { get; private set; } = new List<Location>();
}
