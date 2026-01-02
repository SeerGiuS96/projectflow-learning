using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ProjectFlow.Api.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/auth/login", (LoginRequest request, ITokenService tokenService) =>
        {
            bool isValid = string.Equals(request.Username, "demo", StringComparison.Ordinal)
                && string.Equals(request.Password, "demo", StringComparison.Ordinal);

            if (!isValid)
            {
                return Results.Unauthorized();
            }

            string token = tokenService.CreateToken("user-1", request.Username);
            AuthResponse response = new(token);

            return Results.Ok(response);
        });

        endpoints.MapGet("/me", (ClaimsPrincipal user) =>
        {
            string? username = user.Identity?.Name;
            string? userId = user.FindFirstValue("sub");

            return Results.Ok(new { UserId = userId, Username = username });
        }).RequireAuthorization();

        return endpoints;
    }
}
