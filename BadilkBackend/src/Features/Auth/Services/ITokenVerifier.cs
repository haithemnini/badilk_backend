namespace BadilkBackend.src.Features.Auth.Services;

public interface ITokenVerifier
{
    Task<SocialClaims> VerifyGoogleAsync(string idToken, CancellationToken cancellationToken = default);
}

