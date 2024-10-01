using Aspiregregator;
using Aspiregregator.Frontend.Components;
using Aspiregregator.Frontend.Services;
using Aspiregregator.Frontend.ViewModels;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

// Add front end dependencies
builder.Services.AddSingleton<ISourceProvider, SampleSourceProvider>();
builder.Services.AddScoped<HomePageViewModel>();
builder.Services.AddScoped<EntriesPageViewModel>();
builder.Services.AddScoped<NavMenuViewModel>();
builder.Services.AddScoped<AddNewFeedFormViewModel>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
