using Microsoft.Extensions.Logging;
using GoogleMapsComponents;

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
            String path;
            String key;
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blaze/keys.apikeys");
            if (!File.Exists(path)) {
                File.Create(path);
			}
			try {
                Console.WriteLine(path);
                key = File.ReadAllText(path).Trim();
                builder.Services.AddBlazorGoogleMaps(key);
                Login.Key = key;
				Console.WriteLine(key);
            }
            catch (Exception e) {
                //throw new Exception("Error reading API key at: " + path + " " + e.Message);
                Console.WriteLine("Error reading API key at: " + path + " " + e.Message);
			}
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
