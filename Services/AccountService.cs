namespace plain.Services;

public interface IAccountService
{

    public void Register(int id, string password);

    public void Login(int id, string password);
}

public class AccountService : IAccountService
{
    public void Register(int id, string password)
    {
        // DBに登録
    }

    public void Login(int id, string password)
    {
        // DBから取得
    }
}