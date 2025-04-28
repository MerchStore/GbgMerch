using GbgMerch.Infrastructure.Persistence.Seeding;
using GbgMerch.Infrastructure;
using GbgMerch.Infrastructure.Persistence.Mongo;
using GbgMerch.Application.Cart;
using Microsoft.OpenApi.Models; // ✅ För Swagger
using System.Reflection; // ✅ För XML-dokumentation i Swagger
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);

// Lägg till Infrastructure-tjänster (Repositories, MongoDb osv)
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<ICartService, CartService>();

// Lägg till MVC och Session
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Swagger konfiguration
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

    // Lägg till XML-dokumentation om filen finns
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Felhantering och seeding av testdata i Development
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
    await seeder.SeedAsync();

    // Swagger UI aktiveras i Development
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GbgMerch API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
