using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

public class Downloader {
    private readonly HttpClient _httpClient;
    
    public Downloader (HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<string> DownloadStringAsync(string url) { // Input validation
        if (string.IsNullOrWhiteSpace(url)) {
            return "URL is empty";
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)) {
            // Prepend http:// if the URL does not have a scheme
            if (!url.StartsWith("http://") && !url.StartsWith("https://")) {
                url = "http://" + url;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out uriResult)) {
                return "Invalid URL format";
            }
        }

        try {
            return await _httpClient.GetStringAsync(uriResult);
        }
        catch (HttpRequestException e) {
            return e.Message;
        }
    }
}
