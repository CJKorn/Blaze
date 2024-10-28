using Microsoft.Extensions.Logging;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using System.Threading.Tasks;
using EmbedIO.Actions;
using EmbedIO;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;

namespace Blaze {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<AppState>();
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<Updater>();
            builder.Services.AddSingleton<ReportTools>();

            String key;
            String path = SessionData.path;
            String apiPath = SessionData.apiFilePath;
            String jsonPath = SessionData.jsonFilePath;
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            // Ensure the apiPath directory exists
            var apiDirectory = Path.GetDirectoryName(apiPath);
            if (!string.IsNullOrEmpty(apiDirectory) && !Directory.Exists(apiDirectory)) {
                Directory.CreateDirectory(apiDirectory);
            }
            // Ensure the jsonPath directory exists
            var jsonDirectory = Path.GetDirectoryName(jsonPath);
            if (!string.IsNullOrEmpty(jsonDirectory) && !Directory.Exists(jsonDirectory)) {
                Directory.CreateDirectory(jsonDirectory);
            }

            if (!File.Exists(apiPath)) {
                File.WriteAllText(apiPath, "");
            }
            if (!File.Exists(jsonPath)) {
                File.WriteAllText(jsonPath, "");
            }

            try {
                Console.WriteLine(apiPath);
                key = File.ReadAllText(apiPath).Trim();
                builder.Services.AddBlazorGoogleMaps(key);
                builder.Services.AddScoped(sp => new HttpClient { }); //https://stackoverflow.com/questions/72427377/create-and-use-httpclient-in-a-net-maui-app
                SessionData.Key = key;
                Console.WriteLine(key);
            }
            catch (Exception e) {
                Console.WriteLine("Error reading API key at: " + apiPath + " " + e.Message);
            }

            SessionData.Lat = -33.883239;
            SessionData.Lng = 151.200497;
            SessionData.Zoom = 12;

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            app.Services.GetService<Updater>()?.Start();
			StartHttpServer();

            return app;
        }

        //https://github.com/unosquare/embedio
        private static void StartHttpServer() {
            string hostIpAddress = GetHostIpAddress();
            SessionData.myIP = hostIpAddress + ":9696";
            var server = new WebServer(o => o
                .WithUrlPrefix($"http://{hostIpAddress}:9696/") //Change port if needed, also remember firewall rules
                .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithModule(new ActionModule("/", HttpVerbs.Get, async ctx => {
                    //var jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blaze/data.json");
                    string json = await File.ReadAllTextAsync(SessionData.jsonFilePath);
                    ctx.Response.ContentType = "application/json";
                    await ctx.SendStringAsync(json, "application/json", System.Text.Encoding.UTF8);
                    Stat.users++;
                    Stat.GetReq();
                }))
		        .WithModule(new ActionModule("/report", HttpVerbs.Post, async ctx => {
			        // Receive JSON data via POST request
			        using var reader = new StreamReader(ctx.OpenRequestStream());
			        string receivedJson = await reader.ReadToEndAsync();
                    //SessionData.debug = receivedJson;
                    // Do thingy with received JSON
                    //IReadWrite readWrite = new JsonReadWrite();
                    ReportTools.ReceiveMessage(receivedJson);

					// Respond with a success message
					ctx.Response.ContentType = "application/json";
			        await ctx.SendStringAsync("{\"status\":\"received\"}", "application/json", System.Text.Encoding.UTF8);
                    Stat.ReportUpload();
                }));

			Task.Run(() => server.RunAsync());
        }

        //https://stackoverflow.com/questions/6803073/get-local-ip-address
        private static string GetHostIpAddress() {
			var ipAddress = Dns.GetHostEntry(Dns.GetHostName())
				.AddressList
				.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip)); //lambda expression with LINQ

			return ipAddress?.ToString() ?? throw new Exception("No suitable IP address found.");
		}
    }
}
