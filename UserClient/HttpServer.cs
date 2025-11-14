using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UserClient;

public class HttpServer
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;
    private readonly TcpListener _serverListenter;

    public HttpServer(string ipAddress, int port)
    {
        _ipAddress = IPAddress.Parse(ipAddress);
        _port = port;

        _serverListenter = new TcpListener(_ipAddress, port);
    }

    public void Start()
    {
        Console.WriteLine($"Server started on port {_port}.");
        Console.WriteLine("Listening for requests...");

        while (true)
        {
            _serverListenter.Start();

            var connection = _serverListenter.AcceptTcpClient();

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