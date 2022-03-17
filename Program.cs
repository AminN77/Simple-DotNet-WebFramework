using System.Net;

namespace TempSimpleWebServer;

public class Program
{
    public static readonly HttpListener httpServer = new HttpListener();
    public static readonly List<string> prefixes = new List<string>{"http://+:8080/"};
    public async static Task Main(string[] args)
    {
        ConfigPrefixes();
        await StartServer();
    }

    private async static Task WriteResponse(string data, HttpListenerResponse response)
    {
        var buffer = System.Text.Encoding.UTF8.GetBytes(data);
        response.ContentLength64 = buffer.Length;
        var output = response.OutputStream;
        await output.WriteAsync(buffer,0,buffer.Length);
        output.Close();
    }

    private static void ConfigPrefixes()
    {
        foreach (var p in prefixes)
        {
            httpServer.Prefixes.Add(p);
        }
    }

    private async static Task StartServer()
    {
        httpServer.Start();
        while (true)
        {
            Console.WriteLine("Listening on port 8080 ...");
            var context = await httpServer.GetContextAsync();
            var request = context.Request;
            var response = context.Response;
            await Mapper(request, response);
        }
    }

    private static void StopServer()
    {
        httpServer.Stop();
    }

    private async static Task Mapper(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.Url.ToString().Contains("helloworld"))
        {
            await GetHelloWorld(response);
        }
        else if(request.Url.ToString().Contains("goodbyeworld"))
        {
            await GetGoodByeWorld(response);
        }
    }

    public async static  Task GetHelloWorld(HttpListenerResponse response)
    {
        var data = "<HTML><BODY> Hello world!</BODY></HTML>";
        await WriteResponse(data, response);
    } 

    public async static  Task GetGoodByeWorld(HttpListenerResponse response)
    {
        var data = "<HTML><BODY> Good-Bye world!</BODY></HTML>";
        await WriteResponse(data, response);
    } 
}
