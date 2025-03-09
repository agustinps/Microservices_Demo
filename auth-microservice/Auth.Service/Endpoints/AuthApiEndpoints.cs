using Auth.Service.DTOs;
using Auth.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Service.Endpoints;

public static class AuthApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/login", Login);
    }


    # region POST
    internal static async Task<IResult> Login(
        [FromServices] ITokenService tokenService,
        [FromServices] ILogger<Program> logger,
        LoginRequest loginRequest)
    {
        var loginResult = await tokenService.
                    GenerateAuthenticationToken(loginRequest.Username, loginRequest.Password);
        var message = $"Token generated for user '{loginRequest.Username}'";
        logger.LogInformation(message);
        return loginResult is null
            ? TypedResults.Unauthorized()
            : TypedResults.Ok(loginResult);
    }
    #endregion

}
