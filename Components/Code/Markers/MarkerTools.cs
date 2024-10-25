using GoogleMapsComponents.Maps;

internal class MarkerTools {
    private static readonly IReadWrite readWrite = new JsonReadWrite();
    public MarkerTools() {
    }

    public static LatLngLiteral LatLng(double lat, double lng) {
        LatLngLiteral position = new LatLngLiteral() {
            Lat = lat,
            Lng = lng
        };
        return position;
    }

    public static List<Report> GetReports() {
        SessionData.Reports = readWrite.Deserialize<Report>(SessionData.jsonFilePath);
        return SessionData.Reports;
    }

    public static void AddReport(Report marker) {
        SessionData.Reports.Add(marker);
        SaveReports();
    }

    public static void SaveReports() {
        readWrite.Serialize(SessionData.Reports, SessionData.jsonFilePath);
    }
}
