using Microsoft.AspNetCore.Authentication;

namespace GbgMerch.WebUI.Authentication.ApiKey;

public static class ApiKeyAuthenticationExtensions
{
    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        Action<ApiKeyAuthenticationOptions>? configureOptions = null)
    {
        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
            ApiKeyAuthenticationDefaults.AuthenticationScheme,
            configureOptions);
    }

    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        string apiKey)
    {
        return builder.AddApiKey(options => options.ApiKey = apiKey);
    }
}
