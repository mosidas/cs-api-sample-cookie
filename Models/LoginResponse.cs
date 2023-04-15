namespace plain.Models;

public class LoginResponse
{
    public string Status { get; set; }
    public string Message { get; set; }

    public LoginResponse(string status, string message)
    {
        Status = status;
        Message = message;
    }
}