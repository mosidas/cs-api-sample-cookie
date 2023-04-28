namespace plain.Models;

public class LoginResponse
{
    public string AccessToken { get; set;}

    public LoginResponse(string accessToken)
    {
        AccessToken = accessToken;
    }
}