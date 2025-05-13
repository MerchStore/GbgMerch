namespace GbgMerch.WebUI.Authentication.ApiKey;

public class ApiKeySettings
{
    public string Key { get; set; } = string.Empty;
    public string HeaderName { get; set; } = "X-API-Key";
}
