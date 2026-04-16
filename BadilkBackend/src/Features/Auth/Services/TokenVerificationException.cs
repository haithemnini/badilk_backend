namespace BadilkBackend.src.Features.Auth.Services;

public sealed class TokenVerificationException(string message) : Exception(message);

