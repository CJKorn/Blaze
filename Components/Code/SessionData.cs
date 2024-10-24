using GoogleMapsComponents.Maps;
internal static class SessionData {
    private static readonly IReadWrite readWrite = new JsonReadWrite();
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

    public static LatLngLiteral LatLng(double lat, double lng) {
        LatLngLiteral position = new LatLngLiteral() {
            Lat = lat,
            Lng = lng
        };
        return position;
    }

    public static List<Report> GetReports() {
        Reports = readWrite.Deserialize<Report>(jsonFilePath);
        return Reports;
    }

    public static void AddReport(Report marker) {
        Reports.Add(marker);
        SaveReports();
    }

    public static void SaveReports() {
        readWrite.Serialize(Reports, jsonFilePath);
    }
}
