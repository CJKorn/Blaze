using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

internal class Updater : IDisposable {
	private HttpClient client;
	private IReadWrite readWrite;
	private Downloader downloader;
	private Timer timer;

	public Updater() {
		client = new HttpClient();
		downloader = new Downloader(client);
		readWrite = new JsonReadWrite();
	}

	public void Start() {
		timer = new Timer(UpdateReports, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
	}

	private async void UpdateReports(object state) {
		if (string.IsNullOrEmpty(SessionData.IP)) {
			return;
		}
		string json = await downloader.DownloadStringAsync(SessionData.IP);
		List<Report> reports = readWrite.Deserialize<Report>(json);
		SessionData.Reports = reports;
		File.WriteAllText(SessionData.jsonFilePath, json);
	}

	public void Dispose() {
		client.Dispose();
	}
}
