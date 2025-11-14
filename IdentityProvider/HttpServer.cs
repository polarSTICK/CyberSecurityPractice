using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IdentityProvider;

public class HttpServer
{
    private readonly IPAddress ipAddress;
    private readonly int port;
    private readonly TcpListener serverListenter;

    public HttpServer(string ipAddress, int port)
    {
        this.ipAddress = IPAddress.Parse(ipAddress);
        this.port = port;

        serverListenter = new TcpListener(this.ipAddress, port);
    }

    public void Start()
    {
        Console.WriteLine($"Server started on port {port}.");
        Console.WriteLine("Listening for requests...");

        while (true)
        {
            serverListenter.Start();

            var connection = serverListenter.AcceptTcpClient();

            var networkStream = connection.GetStream();

            WriteResponse(networkStream, "Hello there!");

            connection.Close();
        }
    }

    private void WriteResponse(NetworkStream networkStream, string message)
    {
        var contentLength = Encoding.UTF8.GetByteCount(message);

        var response = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{message}";
        var responseBytes = Encoding.UTF8.GetBytes(response);

        networkStream.Write(responseBytes, 0, responseBytes.Length);
    }
}