internal static class Stat {
    //this class is used to perform all the operations needed for the statistics page, in order to generate the correct statistics
    public static int users { get; set; } = 0;
    public static int usersOutput { get; set; } = 0;
    private static Timer timer;
    private static readonly Dictionary<int, int> getRequestCounts = new Dictionary<int, int>();
    private static readonly Dictionary<int, int> reportUploadCounts = new Dictionary<int, int>();
    //the method below initializes a timer object which triggers the updateusers method every 10 seconds
    public static void Start() {
        timer = new Timer(UpdateUsers, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }
    //this method sets the output value to the current value of users, before reseting users.
    public static void UpdateUsers(object state) {
        usersOutput = users;
        users = 0;
    }
    //this method is used to get the number of reports uploaded within the current hour by looping through the ReportUploadCounts dictionary
    public static void ReportUpload() {
        int currentHour = GetCurrentHour();
        if (reportUploadCounts.ContainsKey(currentHour)) {
            reportUploadCounts[currentHour]++;
        } else {
            reportUploadCounts[currentHour] = 1;
        }
    }
    //the method below gets the number of request counts for the current hour
    public static void GetReq() {
        int currentHour = GetCurrentHour();
        if (getRequestCounts.ContainsKey(currentHour)) {
            getRequestCounts[currentHour]++;
        } else {
            getRequestCounts[currentHour] = 1;
        }
    }
    //the method below is used to get the current hour. Current hour isn't the actual time, but rather a representation of the number of hours
    //since the beginning of .NET time
    public static int GetCurrentHour() {
        TimeSpan timeSpan = DateTime.Now - DateTime.MinValue;
        int totalHours = (int)timeSpan.TotalDays * 24 + DateTime.Now.Hour;
        return totalHours;
    }
    //the method below is used to get the integer that corresponds with the number of get requests for a specific hour.
    public static int GetGetRequestCount(int hour) {
        return getRequestCounts.ContainsKey(hour) ? getRequestCounts[hour] : 0;
    }
    //this method is used to get the integer corresponding with the number of report uploads for a given hour
    public static int GetReportUploadCount(int hour) {
        return reportUploadCounts.ContainsKey(hour) ? reportUploadCounts[hour] : 0;
    }
}
