using Aspirgregator.Abstractions;

namespace FeedUpdater;

public class Worker(ILogger<Worker> logger, IGrainFactory grainFactory) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var sourceLibraryGrain = grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty);
        var sources = await sourceLibraryGrain.GetSourcesAsync();

        if(!sources.Any())
        {
            using var fileStream = File.OpenRead("sample_rss_feeds.txt");
            using var reader = new StreamReader(fileStream);
            var line = await reader.ReadLineAsync();
            while(!string.IsNullOrEmpty(line))
            {
                var newSourceGrain = 
                    await grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty)
                                      .CreateSource(line);

                line = await reader.ReadLineAsync();
            }
        }

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var sourceLibraryGrain = grainFactory.GetGrain<ISourceLibraryGrain>(Guid.Empty);
            var sources = await sourceLibraryGrain.GetSourcesAsync();

            foreach (var source in sources)
            {
                var sourceGrain = await sourceLibraryGrain.GetSourceAsync(source.Endpoint);
                if(sourceGrain is not null)
                {
                    await sourceGrain.UpdateSourceAsync(source);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
