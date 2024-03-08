using System.Net.Sockets;
using FirstHomework.Network.Sender.Response;

namespace FirstHomework.Network.Sender;

public class ClientResponse
{
    private readonly ClientResponse _sender;
    private TcpClient _client = null!;
    private ResponseModel _responseModel = null!;

    public ClientResponse(TcpClient client, int statusCode, string responseContent = "")
        : this(client, new ResponseStatusModel(statusCode), responseContent)
    {

    }
    public ClientResponse(TcpClient client, ResponseStatusModel status, string responseContent = "")
    {
        if (_sender is null)
        {
            _responseModel = new ResponseModel(status, responseContent,
                new HeaderCollection(responseContent));
            _client = client;
            _sender = this;
        }
        else
        {
            _sender._responseModel = new ResponseModel(status, responseContent,
                new HeaderCollection(responseContent));
            _sender._client = client;
        }
    }

    public void AddHeaders(string header, string content)
    {
        _sender._responseModel.Headers.Dict.Add(header, content);
    }

    public async Task SendResponse()
    {
        await using var stream = _sender._client.GetStream();
        await stream.WriteAsync(_responseModel.ToBytes());
    }
}