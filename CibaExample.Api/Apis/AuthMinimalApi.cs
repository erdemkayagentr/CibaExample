using CibaExample.Api.Stores;
using Microsoft.AspNetCore.Mvc;
namespace CibaExample.Api.Apis;

public static class AuthMinimalApi
{

    public static void UseCibaEndPoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("ciba");

        group.MapPost("verify-notification",VerifyAsync).WithName("verify").WithOpenApi();
    }

    private static async Task<IResult> VerifyAsync([FromServices] ICibaRequestStore store,string id)
    {

       await store.CompleteCibaRequestAsync(id);

       return Results.Ok(true);
    }
}
