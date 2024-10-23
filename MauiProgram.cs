using Microsoft.Extensions.Logging;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using System.Threading.Tasks;
using EmbedIO.Actions;
using EmbedIO;
using System.Net;
using System.Diagnostics;

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

            String key;
            String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blaze");
            String apiPath = Path.Combine(path, "keys.apikeys");
            String jsonPath = Path.Combine(path, "data.json");
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
                    var jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blaze/data.json");
                    var json = await File.ReadAllTextAsync(jsonFilePath);
                    ctx.Response.ContentType = "application/json";
                    await ctx.SendStringAsync(json, "application/json", System.Text.Encoding.UTF8);
                }));

            Task.Run(() => server.RunAsync());
        }

        //https://stackoverflow.com/questions/6803073/get-local-ip-address
        private static string GetHostIpAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip)) {
                    return ip.ToString();
                }
            }
            throw new Exception("No suitable IP address found.");
        }
    }
}
