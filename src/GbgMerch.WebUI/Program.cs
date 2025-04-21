using GbgMerch.Infrastructure.Persistence.Seeding;
using GbgMerch.Infrastructure;
using GbgMerch.Infrastructure.Persistence.Mongo; // 👈 För MongoDbSeeder
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
    new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));


var builder = WebApplication.CreateBuilder(args);

// Lägg till Infrastructure-tjänster (Repositories, Mongo etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Lägg till MVC och session
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// Felhantering och seedning
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // 🧪 Lägg till testprodukter i din MongoDB vid utveckling
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<MongoDbSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
