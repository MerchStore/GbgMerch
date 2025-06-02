using Microsoft.AspNetCore.Authentication;

namespace GbgMerch.WebUI.Authentication.ApiKey;

/// <summary>
/// Konfigurationsalternativ för API-nyckelautentisering.
/// </summary>
public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    // Header där klienten måste skicka sin nyckel
    public string HeaderName { get; set; } = ApiKeyAuthenticationDefaults.HeaderName;

    // Nyckeln som servern jämför inkommande nyckel med
    public string ApiKey { get; set; } = string.Empty;
}
