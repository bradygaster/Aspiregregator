namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOrleans(this WebApplicationBuilder builder, Action<ISiloBuilder>? action = null)
    {
        builder.AddKeyedAzureTableClient("clustering", _ => _.DisableTracing = true);
        builder.AddKeyedAzureBlobClient("grainstorage", _ => _.DisableTracing = true);
        builder.UseOrleans(siloBuilder =>
        {
            action?.Invoke(siloBuilder);
        });

        return builder;
    }
}
