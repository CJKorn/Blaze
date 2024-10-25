using GoogleMapsComponents.Maps;
using System.Net.Http.Json;

internal class ReportTools {
    private static readonly IReadWrite readWrite = new JsonReadWrite();
	private static readonly HttpClient client = new HttpClient();

	public ReportTools() {
    }

    public static LatLngLiteral LatLng(double lat, double lng) {
        LatLngLiteral position = new LatLngLiteral() {
            Lat = lat,
            Lng = lng
        };
        return position;
    }

    public static List<Report> GetReports() {
        SessionData.Reports = readWrite.DeserializeFile<Report>(SessionData.jsonFilePath);
        return SessionData.Reports;
    }

    public static void AddReport(Report marker) {
        SessionData.Reports.Add(marker);
        SaveReports();
    }

    public static void SaveReports() {
        readWrite.SerializeFile(SessionData.Reports, SessionData.jsonFilePath);
    }

	public static void ReceiveMessage(string message) {
		List<object> list = readWrite.DeserializeString<object>(message);
		List<Report> reports = new List<Report>();
		try {
			foreach (object obj in list) {
				reports.Add((Report)obj);
			}
		}
		catch (Exception e) {
			Console.WriteLine(e.Message);
		}
		foreach (Report report in reports) {
			SessionData.Reports.Add(report);
		}
		ReportTools.SaveReports();
	}

    public static void SendReports(List<Report> reports) {
		string json = readWrite.SerializeString(reports);
		string url = "http://" + SessionData.IP + "/report/";
		client.PostAsJsonAsync(url, json);
	}
}
