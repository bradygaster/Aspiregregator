var builder = DistributedApplication.CreateBuilder(args);

var sqlserver = builder.AddSqlServer("sqlserver");
var identityDb = sqlserver.AddDatabase("IdentityDb");

builder.AddProject<Projects.Frontend>("frontend")
       .WithReference(identityDb);

builder.Build().Run();
