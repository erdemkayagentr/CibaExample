using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Stores.Serialization;
using System.Security.Claims;

namespace CibaExample.Api.Stores;

public class CibaRequestStore : DefaultGrantStore<BackChannelAuthenticationRequest>, ICibaRequestStore
{

    private IBackchannelAuthenticationInteractionService _cibaInteractionService;
    public CibaRequestStore(string grantType, IPersistedGrantStore store, IPersistentGrantSerializer serializer, IHandleGenerationService handleGenerationService, ILogger logger, IBackchannelAuthenticationInteractionService cibaInteractionService) : base(grantType, store, serializer, handleGenerationService, logger)
    {
        _cibaInteractionService = cibaInteractionService;
    }


    public async Task CompleteCibaRequestAsync(string id)
    {
       var persisted = await Store.GetAsync(id);
       if (persisted == null)
       {
           throw new NullReferenceException("Grant not found");
       }
       var backChannelAuthenticationRequestData = Serializer.Deserialize<BackChannelAuthenticationRequest>(persisted.Data);
        var completeRequest = new CompleteBackchannelLoginRequest(id)
       {
           InternalId = id,
           Subject = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
               new Claim("sub", persisted.SubjectId) // Kullanıcıyı tanımlayan subject (örneğin user_id)
           })),
           ScopesValuesConsented = backChannelAuthenticationRequestData.RequestedScopes
        };

       await _cibaInteractionService.CompleteLoginRequestAsync(completeRequest);
        
       
    }
  
}
