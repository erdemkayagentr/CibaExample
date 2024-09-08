
using System.Security.Claims;
using Duende.IdentityServer.Validation;
using IdentityModel;

namespace CibaExample.Api.Validators;

public class BackchannelAuthenticationUserValidator : IBackchannelAuthenticationUserValidator
{

    public Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext)
    {

        var user = TestUsers.Users.FirstOrDefault(u => u.Username == userValidatorContext?.LoginHint);

        return Task.FromResult(new BackchannelAuthenticationUserValidationResult
        {
            Subject = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("sub", user?.SubjectId),
                new Claim(JwtClaimTypes.AuthenticationTime, DateTime.UtcNow.ToEpochTime().ToString()), // auth_time claim'i
                new Claim(JwtClaimTypes.IdentityProvider, "local"),
            })),
        });
    }
}
