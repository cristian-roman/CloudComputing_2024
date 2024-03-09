using System.Net.Sockets;
using FirstHomework.Network.Config;
using FirstHomework.Network.Resolver.RequestProcessor;
using FirstHomework.Network.Resolver.RequestProcessor.Exceptions;
using FirstHomework.Network.Sender;

namespace FirstHomework.Network;

public class ServerNetwork(string networkConfigFilePath)
{
    private readonly NetworkConfigModel _configModel = NetworkConfigLoader.ParseConfigFile(networkConfigFilePath);

    public async Task StartServer()
    {
        var tcpListener = new TcpListener(_configModel.Host, _configModel.Port);
        tcpListener.Start();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Server started at " + _configModel.Host + ":" + _configModel.Port);

        while (true)
        {
            Console.WriteLine("Waiting for a connection...");
            var client = await tcpListener.AcceptTcpClientAsync();
            Console.WriteLine("Client with IP " + client.Client.RemoteEndPoint + " connected");

            ClientResponse? responseSender = null;
            try
            {
                var request = await RequestProcessor.ParseRequest(client);

                var route = RequestProcessor.RouteRequest(request);

                var message = route(request);
                responseSender = new ClientResponse(client, 200, message);
            }
            catch (RequestProcessingException e)
            {
                responseSender = new ClientResponse(client, e.Status, e.Message);
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                responseSender = new ClientResponse(client, 500);
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    if (responseSender is not null)
                    {
                        responseSender.AddHeaders("Server", "FirstHomework");
                        await responseSender.SendResponse();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                client.Close();
            }

        }
    }
}