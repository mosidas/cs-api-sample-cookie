namespace plain.Models;

public class LogoutResponse
{
    public string Message { get; set; }

    public LogoutResponse(string message)
    {
        Message = message;
    }
}