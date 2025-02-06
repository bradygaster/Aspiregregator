using Aspiregregator;
using Aspiregregator.Frontend.Components;
using Aspiregregator.Frontend.Services;
using Aspiregregator.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents(
    static options => options.UseTooltipServiceProvider = true);

// Add front end dependencies
builder.Services.AddSingleton<ISourceProvider, SampleSourceProvider>();
builder.Services.AddScoped<HomePageViewModel>();
builder.Services.AddScoped<EntriesPageViewModel>();
builder.Services.AddScoped<NavMenuViewModel>();
builder.Services.AddScoped<AddNewFeedFormViewModel>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddHttpClient();

builder.AddOrleans();

var app = builder.Build();

app.MapDefaultEndpoints();

// Ensure the database is created and up-to-date.
// Learn more at https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
await using (var scope = app.Services.CreateAsyncScope())
{
    //var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("api/proxy/image", async ([FromQuery] Uri url, IHttpClientFactory factory) =>
{
    var response = await factory.CreateClient().GetAsync(url);
    var contentType = response.Content.Headers.ContentType?.ToString();
    var content = await response.Content.ReadAsByteArrayAsync();

    return Results.File(content, contentType);
});

app.Run();
