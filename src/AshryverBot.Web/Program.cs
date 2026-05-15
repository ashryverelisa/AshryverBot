using AshryverBot.Database;
using AshryverBot.Infrastructure;
using AshryverBot.Twitch;
using AshryverBot.Web.Authentication;
using AshryverBot.Web.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddMudServices();

var dataProtectionKeyPath = builder.Configuration["DataProtection:KeyPath"];
if (!string.IsNullOrWhiteSpace(dataProtectionKeyPath))
{
    Directory.CreateDirectory(dataProtectionKeyPath);
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyPath))
        .SetApplicationName("AshryverBot");
}

builder.Services.AddAshryverBotDatabase(builder.Configuration);
builder.Services.AddTwitchClients(builder.Configuration);
builder.Services.AddAshryverBotInfrastructure(builder.Configuration);
builder.Services.AddTwitchLogin(builder.Configuration);

var app = builder.Build();

var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
await using (var db = await dbContextFactory.CreateDbContextAsync())
{
    await db.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
