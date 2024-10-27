using GoogleMapsComponents.Maps;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices.Sensors;
using Swan.Formatters;

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

    public static void SaveReports(List<Report> reports) {
        readWrite.SerializeFile(reports, SessionData.jsonFilePath);
    }

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

	public static List<Mono> GetMonos(List<Report> reports) {
        List<Mono> Monos = new List<Mono>();
        foreach (Report report in reports) {
            if (report is Mono mono) {
                Monos.Add(mono);
            }
        }
        return Monos;
    }

    public static List<Poly> GetPolys(List<Report> reports) {
        List<Poly> Polys = new List<Poly>();
        foreach (Report report in reports) {
            if (report is Poly poly) {
                Polys.Add(poly);
            }
        }
        return Polys;
    }

    public static void DeleteReport(Report report) {
        SessionData.Reports.Remove(report);
        SaveReports();
    }
}
