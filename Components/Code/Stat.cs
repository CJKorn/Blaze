internal static class Stat {
    public static int users { get; set; } = 0;
    public static int usersOutput { get; set; } = 0;
    private static Timer timer;
    private static readonly Dictionary<int, int> getRequestCounts = new Dictionary<int, int>();
    private static readonly Dictionary<int, int> reportUploadCounts = new Dictionary<int, int>();

    public static void Start() {
        timer = new Timer(UpdateUsers, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    public static void UpdateUsers(object state) {
        usersOutput = users;
        users = 0;
    }

    public static void ReportUpload() {
        int currentHour = GetCurrentHour();
        if (reportUploadCounts.ContainsKey(currentHour)) {
            reportUploadCounts[currentHour]++;
        } else {
            reportUploadCounts[currentHour] = 1;
        }
    }

    public static void GetReq() {
        int currentHour = GetCurrentHour();
        if (getRequestCounts.ContainsKey(currentHour)) {
            getRequestCounts[currentHour]++;
        } else {
            getRequestCounts[currentHour] = 1;
        }
    }

    public static int GetCurrentHour() {
        TimeSpan timeSpan = DateTime.Now - DateTime.MinValue;
        int totalHours = (int)timeSpan.TotalDays * 24 + DateTime.Now.Hour;
        return totalHours;
    }

    public static int GetGetRequestCount(int hour) {
        return getRequestCounts.ContainsKey(hour) ? getRequestCounts[hour] : 0;
    }

    public static int GetReportUploadCount(int hour) {
        return reportUploadCounts.ContainsKey(hour) ? reportUploadCounts[hour] : 0;
    }
}
