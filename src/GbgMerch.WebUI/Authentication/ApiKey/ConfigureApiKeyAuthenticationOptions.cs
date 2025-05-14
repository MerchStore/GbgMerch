using Microsoft.Extensions.Options;

namespace GbgMerch.WebUI.Authentication.ApiKey;

public class ConfigureApiKeyAuthenticationOptions : IConfigureOptions<ApiKeyAuthenticationOptions>
{
    private readonly IConfiguration _configuration;

    public ConfigureApiKeyAuthenticationOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ApiKeyAuthenticationOptions options)
    {
        options.ApiKey = _configuration["ApiKeySettings:ApiKey"];
        options.HeaderName = "X-API-Key"; // Om du vill ha det statiskt här
    }
}
