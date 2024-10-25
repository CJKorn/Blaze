using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

public class Downloader {
    private readonly HttpClient _httpClient;
    
    public Downloader (HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<string> DownloadStringAsync(string url) {
        if (string.IsNullOrWhiteSpace(url)) {
			return "URL is empty";
		}
		try {
            return await _httpClient.GetStringAsync(url);
        }
        catch (HttpRequestException e) {
            return e.Message;
        }
    }
}
