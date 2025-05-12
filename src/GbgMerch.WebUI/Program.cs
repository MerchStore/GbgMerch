
using GbgMerch.Infrastructure.Persistence.Seeding;
using GbgMerch.Infrastructure;
using GbgMerch.Infrastructure.Persistence.Mongo;
using GbgMerch.Application.Cart;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using GbgMerch.WebUI.Infrastructure;
using GbgMerch.WebUI.Authentication.ApiKey;

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

//
// 🧱 Registrera apptjänster
//
builder.Services.AddInfrastructure(builder.Configuration); // MongoDB, Repos etc.
builder.Services.AddSingleton<ICartService, CartService>(); // Ex. kundvagn

// 🧠 Session-konfiguration
builder.Services.AddDistributedMemoryCache(); // 🧠 Lagring för session i RAM
builder.Services.AddSession();                // 🛒 Aktiverar session

//
// 🐍 JSON-format med snake_case och enums som strängar (för API och JS-klienter)
//
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.DictionaryKeyPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//
// 💳 API-nyckel-autentisering
//
builder.Services.AddAuthentication()
    .AddApiKey(builder.Configuration["ApiKey:Value"]
    ?? throw new InvalidOperationException("API Key saknas i appsettings.json"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser());
});

//
// 🌍 CORS – tillåt externa JS-appar att anropa API:t (t.ex. client/index.html)
/*
 * OBS! I produktion ska du INTE använda AllowAnyOrigin – du ska ange exakt domän!
 */
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//
// 📘 Swagger-dokumentation
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GbgMerch API",
        Version = "v1",
        Description = "API för GbgMerch produktkatalog och beställningar.",
        Contact = new OpenApiContact
        {
            Name = "GbgMerch Support",
            Email = "support@gbgmerch.se"
        }
    });

    // Lägg till XML-dokumentation om den finns
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // 🔐 Swagger UI ska stödja API Key-autentisering
    options.AddSecurityDefinition(ApiKeyAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "Skriv in din API-nyckel.",
        Name = ApiKeyAuthenticationDefaults.HeaderName,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ApiKeyAuthenticationDefaults.AuthenticationScheme
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//
// 🚀 Bygg appen
//
var app = builder.Build();

//
// 🌱 Seed testdata + Swagger UI i utvecklingsläge
//
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
    await seeder.SeedAsync();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GbgMerch API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//
// 🧭 Middleware pipeline – ordning är viktigt!
//
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseCors("AllowAllOrigins");       // 🌍 Tillåt cross-origin JS-anrop
app.UseAuthentication();              // 🔐 Kör autentisering (API Key)
app.UseAuthorization();               // ✅ Kör [Authorize]-policies

//
// 🧭 Routing
//
app.MapControllers(); // API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // MVC

app.Run();