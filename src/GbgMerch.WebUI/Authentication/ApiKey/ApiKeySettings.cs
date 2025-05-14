namespace GbgMerch.WebUI.Authentication.ApiKey;

public class ApiKeySettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string HeaderName { get; set; } = "X-API-Key";
}
