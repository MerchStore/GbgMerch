using Microsoft.AspNetCore.Authentication;

namespace GbgMerch.WebUI.Authentication.ApiKey;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = ApiKeyAuthenticationDefaults.HeaderName;
    public string ApiKey { get; set; } = string.Empty;
}
