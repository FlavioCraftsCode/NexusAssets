using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using NexusAssets.Application.Common.Interfaces;
using NexusAssets.Infrastructure.Persistence; 
using NexusAssets.Infrastructure.Services;
using NexusAssets.Server.UI.Components;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=NexusAssets.db";


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString, b => 
        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


builder.Services.AddScoped<IApplicationDbContext>(provider => 
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        if (context.Database.IsRelational())
        {
            
            context.Database.Migrate();
        }

        
        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao migrar ou popular o banco de dados.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();