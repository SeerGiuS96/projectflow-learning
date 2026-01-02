namespace ProjectFlow.Api.Auth;

public sealed class AuthResponse
{
    public AuthResponse(string accessToken)
    {
        AccessToken = accessToken;
    }

    public string AccessToken { get; }
}
