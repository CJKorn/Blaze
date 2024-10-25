using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

//Unused, kept for future use
internal class Reciever {
	IReadWrite readWrite = new JsonReadWrite();
	[Route(HttpVerbs.Get, "/send-message/{message}")]
	public string ReceiveMessage(string message) {
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
		return $"Received: {message}";
	}
}
