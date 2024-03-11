using FirstHomework.Network.Sender.Response;

namespace FirstHomework.APIs;

public class APIResponse(string message, ResponseStatusModel status)
{
    public string Message { get; set; } = message;
    public ResponseStatusModel Status { get; set; } = status;
}