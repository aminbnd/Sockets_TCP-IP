using System.Net;
using System.Net.Sockets;
using System.Text;

string hostName = Dns.GetHostName();
IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(hostName);
IPAddress ipAdress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAdress, 11_000);

using Socket client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
await client.ConnectAsync(ipEndPoint);


while (true)
{
    sendMessage();
    receiveMessage();
}




async void sendMessage()
{
    var message = Console.ReadLine();
    try
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
        Console.WriteLine($"Me: {message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

async void receiveMessage()
{
    var buffer = new byte[1024];
    try
    {
        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        Console.WriteLine($"Server: {response}");
    }
    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
}