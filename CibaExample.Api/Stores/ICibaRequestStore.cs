namespace CibaExample.Api.Stores;

public interface ICibaRequestStore
{
    Task CompleteCibaRequestAsync(string id); //Bekleyen Ciba isteğini tamamlar
}
