using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
//this class is used to fetch and update the report data at a period of every 10 seconds
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
	//this method runs when the application starts. it sets a timer that updates all data from the host device every 10 seconds
	public void Start() {
		Stat.Start();
		if (SessionData.hosting) {
			return;
		}
		timer = new Timer(UpdateReports, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
	}
	//the method below is what actually updates the reports, if the user is not hosting, and the ip isn't empty.
	//it wokrs by downloading a json string from SessionData, and deserializing it, before writing al the data to the reports file.
	private async void UpdateReports(object state) {
		if (SessionData.hosting) {
			SessionData.Reports = ReportTools.GetReports();
			return;
		}
		if (string.IsNullOrEmpty(SessionData.IP)) {
			return;
		}
		string json = await downloader.DownloadStringAsync(SessionData.IP);
		List<Report> reports = readWrite.DeserializeFile<Report>(json);
		SessionData.Reports = reports;
		File.WriteAllText(SessionData.jsonFilePath, json);
	}
	//used to clean up the HTTPClient resource, good code practice to include this
	public void Dispose() {
		client.Dispose();
	}
}
