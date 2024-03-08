namespace FirstHomework.Network.Sender.Exceptions;

public class UnsupportedStatusCodeException(int statusCode):
Exception($"The status code {statusCode} is not supported.");