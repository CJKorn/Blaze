using EmbedIO.Actions;
using EmbedIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

internal class EIOHTTPServer : IHTTPServer {
    public void StartHttpServer() {
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

    private static string GetHostIpAddress() {
        var ipAddress = Dns.GetHostEntry(Dns.GetHostName())
            .AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip)); //lambda expression with LINQ

        return ipAddress?.ToString() ?? throw new Exception("No suitable IP address found.");
    }
}