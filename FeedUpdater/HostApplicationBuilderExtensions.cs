namespace Microsoft.Extensions.DependencyInjection;

internal static class HostApplicationBuilderExtensions
{
    public static HostApplicationBuilder AddOrleans(this HostApplicationBuilder builder, Action<ISiloBuilder>? action = null)
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
