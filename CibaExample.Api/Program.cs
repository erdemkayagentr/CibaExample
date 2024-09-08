using CibaExample.Api;
using CibaExample.Api.Apis;
using CibaExample.Api.Services;
using CibaExample.Api.Stores;
using CibaExample.Api.Validators;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Stores.Serialization;
using IdentityModel;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddIdentityServer(opt =>
    {
        opt.Events.RaiseErrorEvents = true;
        opt.Events.RaiseSuccessEvents = true;
        opt.Events.RaiseFailureEvents = true;
        opt.Ciba.DefaultLifetime = 600;
    })
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "ciba_client",
            AllowedGrantTypes = { OidcConstants.GrantTypes.Ciba, OidcConstants.GrantTypes.RefreshToken},
            RefreshTokenExpiration = TokenExpiration.Sliding,
            AllowOfflineAccess = true,
            AllowAccessTokensViaBrowser = false,
            CoordinateLifetimeWithUserSession = true,
            UpdateAccessTokenClaimsOnRefresh = true,
            CibaLifetime = 180,
            RefreshTokenUsage = TokenUsage.ReUse,
            Enabled = true,
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = {"openid", "profile", "api" },
            AccessTokenLifetime = 180,
            IdentityTokenLifetime = 180
            
        }
    })
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    })
    .AddInMemoryApiScopes(new List<ApiScope>
    {
        new ApiScope("api", "Ciba Api")
    }).AddTestUsers(TestUsers.Users)
    .AddBackchannelAuthenticationUserValidator<BackchannelAuthenticationUserValidator>()
    .AddBackchannelAuthenticationUserNotificationService<BackchannelAuthenticationUserNotificationService>()
    .AddDeveloperSigningCredential();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICibaRequestStore, CibaRequestStore>(serviceProvider =>
{
    var store = serviceProvider.GetRequiredService<IPersistedGrantStore>();
    var serializer = serviceProvider.GetRequiredService<IPersistentGrantSerializer>();
    var handleGenerationService = serviceProvider.GetRequiredService<IHandleGenerationService>();
    var logger = serviceProvider.GetRequiredService<ILogger<CibaRequestStore>>();
    var cibaInteractionService = serviceProvider.GetRequiredService<IBackchannelAuthenticationInteractionService>();

    return new CibaRequestStore(OidcConstants.GrantTypes.Ciba, store, serializer, handleGenerationService, logger, cibaInteractionService);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseIdentityServer();
app.UseCibaEndPoints();
app.Run();
