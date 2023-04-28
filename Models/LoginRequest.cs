namespace plain.Models;

public class LoginRequest
{
    public int Id { get; set; }
    public string Password { get; set; }

    public LoginRequest(int id, string password)
    {
        Id = id;
        Password = password;
    }

    public string ToText()
    {
        return $"Id: {Id}, Password: {Password}";
    }
}
