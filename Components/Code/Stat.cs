internal static class Stat {
    private static readonly Dictionary<int, int> hourlyCounts = new Dictionary<int, int>();
    public static int users { get; set; } = 0;
    public static int usersOutput { get; set; } = 0;

    public static void Start() {
        Timer timer = new Timer(UpdateUsers, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    public static void UpdateUsers(object state) {
        usersOutput = users;
        users = 0;
    }

    public static void IncrementHourCount(int hour) {
        if (hourlyCounts.ContainsKey(hour)) {
            hourlyCounts[hour]++;
        } else {
            hourlyCounts[hour] = 1;
        }
    }

    public static int GetHourCount(int hour) {
        return hourlyCounts.ContainsKey(hour) ? hourlyCounts[hour] : 0;
    }

    public static void ReportUpload() {
        //
    }

    public static void GetReq() {
        //
    }
}
