// Importerar verktyg för att manipulera text och JSON
using System.Text;
using System.Text.Json;

namespace GbgMerch.WebUI.Infrastructure;

// En klass som bestämmer hur egenskapsnamn ska skrivas i JSON (snake_case)
public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    // Denna metod tar ett namn som t.ex. "ProductName" och gör om det till "product_name"
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name; // Om inget namn, returnera som det är

        var builder = new StringBuilder();

        // Går igenom varje tecken i namnet
        for (int i = 0; i < name.Length; i++)
        {
            // Om det är en versal och inte första tecknet, lägg till ett understreck först
            if (i > 0 && char.IsUpper(name[i]))
                builder.Append('_');

            // Lägg till bokstaven som gemen (liten bokstav)
            builder.Append(char.ToLowerInvariant(name[i]));
        }

        // Returnerar t.ex. "ProductName" => "product_name"
        return builder.ToString();
    }
}
