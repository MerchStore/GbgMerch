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
        // 🟡 HÄR placeras den
        options.ApiKey = _configuration["ApiKeySettings:ApiKey"] ?? string.Empty;

        // 🔐 HeaderName (kan tas från config också om du vill)
        options.HeaderName = "X-API-Key";

        // 💡 Valfri debug-logg (kan tas bort senare)
        Console.WriteLine($"🔐 [DEBUG] Loaded API key from config: {options.ApiKey}");
    }
}
