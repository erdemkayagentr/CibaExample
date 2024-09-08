using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace CibaExample.Api.Services;

public class BackchannelAuthenticationUserNotificationService : IBackchannelAuthenticationUserNotificationService
{
    public Task SendLoginRequestAsync(BackchannelUserLoginRequest request)
    {
        Console.WriteLine($"**Sending login request to user with request ID {request.InternalId}");
        return Task.CompletedTask;
    }
}
