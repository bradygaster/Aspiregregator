var builder = DistributedApplication.CreateBuilder(args);

var sqlserver = builder.AddSqlServer("sqlserver");
var identityDb = sqlserver.AddDatabase("IdentityDb");

var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator();

var clustering = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grainstorage");

var orleans = builder.AddOrleans("orleans-cluster")
                     .WithClustering(clustering)
                     .WithGrainStorage("FeedSourceLibrary", grainStorage)
                     .WithGrainStorage("FeedSource", grainStorage);

builder.AddProject<Projects.Frontend>("frontend")
       .WithReference(identityDb)
       .WithReference(orleans);

builder.Build().Run();
