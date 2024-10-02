using Aspirgregator.Abstractions;

namespace FeedUpdater;

public class Worker(ILogger<Worker> logger, IGrainFactory grainFactory) : BackgroundService
{
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

            await Task.Delay(5000, stoppingToken);
        }
    }
}
