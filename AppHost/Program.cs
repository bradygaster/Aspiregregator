var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator();

var clustering = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grainstorage");

var orleans = builder.AddOrleans("orleans-cluster")
                     .WithClustering(clustering)
                     .WithGrainStorage("FeedSourceLibrary", grainStorage)
                     .WithGrainStorage("FeedSource", grainStorage);

builder.AddProject<Projects.FeedUpdater>("feedupdater")
       .WithReference(orleans);

builder.AddProject<Projects.Frontend>("frontend")
       .WithReference(orleans)
       .WithExternalHttpEndpoints();

builder.Build().Run();