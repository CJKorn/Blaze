using System.IO;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Actions;
//this class is used to call the method that starts the HTTP Server
internal interface IHTTPServer {
    public void StartHttpServer();
}
