
using System.Net;
using System.Net.Sockets;
using System.Text;

string hostName = Dns.GetHostName();
IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(hostName);
IPAddress ipAdress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAdress, 11_000);

using Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
listener.Bind(ipEndPoint);
listener.Listen(1000);
Console.WriteLine("Waiting connection ...");
var handeler = await listener.AcceptAsync();
if (handeler != null) Console.WriteLine("Succefully connected"); else Console.WriteLine("Connection failed");


while (true)
{
    receiveMessage();
    sendMessage();
}





async void receiveMessage()
{
    var buffer = new byte[1024];
    try
    {
        var received = await handeler.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        Console.WriteLine($"Client: {response}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

async void sendMessage()
{
    var message = Console.ReadLine();

    try
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        _ = await handeler.SendAsync(messageBytes, SocketFlags.None);
        Console.WriteLine($"Server: {message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}