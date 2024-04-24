using System.Text.Json;

namespace FirstHomework.Network.Config;

public class NetworkConfigLoader
{
    public static NetworkConfigModel ParseConfigFile(string networkConfigFilePath)
    {
        try
        {
            var file = File.ReadAllText(networkConfigFilePath);
            return JsonSerializer.Deserialize<NetworkConfigModel>(file) ?? throw new JsonException();
        }
        catch (JsonException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Network config file is not in the correct format.\n" +
                              "Check parsing class and configuration file");
            Console.ResetColor();
        }
        catch (ArgumentNullException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Network config file is empty");
            Console.ResetColor();
        }
        catch (IOException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Network config file could not be opened or read. Check the rights of the file");
            Console.ResetColor();
        }

        throw new ArgumentException("Network config file could not be parsed");
    }
}