using FeedUpdater;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddOrleans();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
