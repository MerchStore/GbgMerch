using GbgMerch.Infrastructure;
using GbgMerch.Infrastructure.Persistence.Seeding;
using GbgMerch.Infrastructure.Persistence.Mongo;
using GbgMerch.Application.Cart;
using GbgMerch.WebUI.Infrastructure;
using GbgMerch.WebUI.Authentication.ApiKey;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using GbgMerch.Application;

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

//
// 🧱 Registrera tjänster
//
builder.Services.AddInfrastructure(builder.Configuration); // MongoDB, repos, external services
builder.Services.AddApplication();
builder.Services.AddSingleton<ICartService, CartService>();

// 🧠 Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//
// 💳 API-key authentication
//
builder.Services.Configure<ApiKeySettings>(builder.Configuration.GetSection("ApiKeySettings"));

builder.Services.ConfigureOptions<ConfigureApiKeyAuthenticationOptions>();

builder.Services.AddAuthentication(ApiKeyAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationDefaults.AuthenticationScheme, options => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser());
});

//
// 🌍 CORS
//
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
// 🐍 JSON-format + enum string
//
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.DictionaryKeyPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//
// 📘 Swagger
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GbgMerch API",
        Version = "v1",
        Description = "API för GbgMerch produktkatalog och recensioner.",
        Contact = new OpenApiContact
        {
            Name = "GbgMerch Support",
            Email = "support@gbgmerch.se"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // 🔐 API-nyckel setup för Swagger UI
    options.AddSecurityDefinition(ApiKeyAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "Ange din API-nyckel i headern",
        Name = ApiKeyAuthenticationDefaults.HeaderName, // "X-API-Key"
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ApiKeyAuthenticationDefaults.AuthenticationScheme
    });

    // ✅ Lägg till detta block (DETTA visar 🔒-knappen i Swagger UI)
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = ApiKeyAuthenticationDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });

    // 👇 Endast nödvändigt om du har en custom OperationFilter (t.ex. för kommentarer)
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//
// 🚀 Bygg appen
//
var app = builder.Build();

//
// 🌱 Seed testdata i utvecklingsläge
//
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
    await seeder.SeedAsync();
}

// Swagger alltid aktivt
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GbgMerch API V1");
    options.RoutePrefix = "swagger"; // Detta gör att /swagger fungerar
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


//
// 🧭 Middleware pipeline
//
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();