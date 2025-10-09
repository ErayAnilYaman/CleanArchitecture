var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CleanArchitecture_WebApi>("cleanarchitecture-webapi");

builder.Build().Run();
