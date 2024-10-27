using GoogleMapsComponents.Maps;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices.Sensors;
using Swan.Formatters;
//this file defines the various tools and components needed in order to create a report
internal class ReportTools {
    private static readonly IReadWrite readWrite = new JsonReadWrite();
    private static readonly HttpClient client = new HttpClient();
    private static readonly ILogger<ReportTools> _logger = new LoggerFactory().CreateLogger<ReportTools>();

    public ReportTools() {
    }

    public static LatLngLiteral LatLng(double lat, double lng) {
        LatLngLiteral position = new LatLngLiteral() {
            Lat = lat,
            Lng = lng
        };
        return position;
    }
    //defines a getter that returns a list of the reports
    public static List<Report> GetReports() {
        SessionData.Reports = readWrite.DeserializeFile<Report>(SessionData.jsonFilePath);
        return SessionData.Reports;
    }
    //adds a report at the current marker to SessionData using the current marker, and saves it.
    public static void AddReport(Report marker) {
        SessionData.Reports.Add(marker);
        SaveReports();
    }
    //saves the reports to the .JSON file and doesnt take any parameters
    public static void SaveReports() {
        readWrite.SerializeFile(SessionData.Reports, SessionData.jsonFilePath);
    }
    //saves the reports to the .JSON file and takes the list of report objects as a parameter
    public static void SaveReports(List<Report> reports) {
        readWrite.SerializeFile(reports, SessionData.jsonFilePath);
    }
    //takes a serialized message of reports, deserializes it, and stores them
    public static void ReceiveMessage(string message) {
        ReportTools.GetReports();
        SessionData.debug = "";
        List<object> list = readWrite.DeserializeString<object>(message);
		File.WriteAllText(SessionData.jsonFilePath + "a", message);
		List<Report> reports = new List<Report>();
        try {
            foreach (object obj in list) {
                if (obj is Report report) {
                    reports.Add(report);
                    SessionData.debug += report.ToString() + "\n";
				} else {
                    _logger.LogWarning($"Object of type {obj.GetType()} is not a Report or derived from Report");
                    SessionData.debug += $"Object of type {obj.GetType()} is not a Report or derived from Report \n";
				}
            }
        }
        catch (Exception e) {
            _logger.LogError(e, "Error while processing received message");
			SessionData.debug += e.Message + "\n";
		}
        foreach (Report report in reports) {
            SessionData.Reports.Add(report);
        }
        ReportTools.SaveReports();
        //SessionData.Zoom = 1;
    }

    //Serializes the list of report objects, and prepares them for sending, before validating the location and finally sending them.
    public static void SendReports(List<Report> reports) {
        string json = readWrite.SerializeString(reports);
		string ip = SessionData.IP;

        if (string.IsNullOrWhiteSpace(ip)) {
            _logger.LogError("No IP address provided");
            return;
        }

        // Ensure the IP includes the port number
        string url = $"http://{ip}/report/";
        _logger.LogInformation($"Sending reports to {url}");

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)) {
            _logger.LogError("Invalid IP address format");
            return;
        }

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        client.PostAsync(uriResult, content);
    }
    //this method takes an individual report object, adds it to a list of report objects then serializes that list, before
    // preparing it for sending, validating the location and then sending it to the location.
	public static void SendReports(Report report) {
        List<Report> reports = new List<Report> { report };
		string json = readWrite.SerializeString(reports);
		string ip = SessionData.IP;

		if (string.IsNullOrWhiteSpace(ip)) {
			_logger.LogError("No IP address provided");
			return;
		}

		// Ensure the IP includes the port number
		string url = $"http://{ip}/report/";
		_logger.LogInformation($"Sending reports to {url}");

		if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)) {
			_logger.LogError("Invalid IP address format");
			return;
		}

		var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
		client.PostAsync(uriResult, content);
	}
    //retrieves a list of Mono Objects based off of the list of Report objects
	public static List<Mono> GetMonos(List<Report> reports) {
        List<Mono> Monos = new List<Mono>();
        foreach (Report report in reports) {
            if (report is Mono mono) {
                Monos.Add(mono);
            }
        }
        return Monos;
    }
    //retrieves a list of Poly objects, based off of the list of Report objects.
    public static List<Poly> GetPolys(List<Report> reports) {
        List<Poly> Polys = new List<Poly>();
        foreach (Report report in reports) {
            if (report is Poly poly) {
                Polys.Add(poly);
            }
        }
        return Polys;
    }
    //deletes a report using SessionData, before saving the reports.
    public static void DeleteReport(Report report) {
        SessionData.Reports.Remove(report);
        SaveReports();
    }
}
