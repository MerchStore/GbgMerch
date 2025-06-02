using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace GbgMerch.WebUI.Authentication.ApiKey;

/// <summary>
/// Verifierar att inkommande anrop innehåller en giltig API-nyckel.
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    // Här sker själva autentiseringen
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Kontroll: finns headern med?
        if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues))
        {
            Logger.LogWarning("API key saknas i header '{HeaderName}'", Options.HeaderName);
            return Task.FromResult(AuthenticateResult.Fail("API key saknas."));
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            Logger.LogWarning("API key är tom.");
            return Task.FromResult(AuthenticateResult.Fail("API key är tom."));
        }

        // Kontrollera att nyckeln matchar den konfigurerade
        if (providedApiKey != Options.ApiKey)
        {
            Logger.LogWarning("Felaktig API key: {ProvidedKey}", providedApiKey);
            return Task.FromResult(AuthenticateResult.Fail("Ogiltig API key."));
        }

        // Nyckeln är korrekt – skapa claims och autentiseringsticket
        var claims = new[] { new Claim(ClaimTypes.Name, "API Client") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        Logger.LogInformation("API key-autentisering lyckades.");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
