using Microsoft.AspNetCore.Authentication;

namespace GbgMerch.WebUI.Authentication.ApiKey;

/// <summary>
/// Extension-metoder för att förenkla registrering av API-nyckelautentisering.
/// </summary>
public static class ApiKeyAuthenticationExtensions
{
    /// <summary>
    /// Registrerar API-nyckelautentisering med möjlighet att konfigurera options.
    /// </summary>
    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        Action<ApiKeyAuthenticationOptions>? configureOptions = null)
    {
        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationDefaults.AuthenticationScheme,
            configureOptions);
    }

    /// <summary>
    /// Registrerar API-nyckelautentisering med direkt inmatning av nyckel.
    /// </summary>
    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        string apiKey)
    {
        return builder.AddApiKey(options => options.ApiKey = apiKey);
    }
}
