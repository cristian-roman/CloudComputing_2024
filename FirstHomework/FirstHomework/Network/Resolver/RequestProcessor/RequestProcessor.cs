using System.Net.Sockets;
using System.Text;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions.Request;

namespace FirstHomework.Network.Resolver.RequestProcessor;

public static class RequestProcessor
{
    public static async Task<RequestModel> ParseRequest(TcpClient client)
    {
        var stream = client.GetStream();
        var buffer = new byte[4096];
        var byteCount = await stream.ReadAsync(buffer);

        if (byteCount == 0)
        {
            throw new EmptyRequestException();
        }

        var request = Encoding.UTF8.GetString(buffer, 0, byteCount);
        return new RequestModel(request);
    }

}