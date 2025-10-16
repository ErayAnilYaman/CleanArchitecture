using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;
using CleanArchitecture.WebApi.Modules;
using CleanArchitecture.WebApi.Modules.Employees;
using CleanArchitecture.WebApi.Error;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddOData(opt =>
opt.Select().
Filter()
.Count()
.OrderBy()
.Expand()
.SetMaxTop(1000)
.AddRouteComponents("odata" , AppODataController.GetEdmModel())


);


builder.Services.AddRateLimiter(x=>x.AddFixedWindowLimiter("fixed" , cfg =>
{
    cfg.QueueLimit = 100;
    cfg.Window = TimeSpan.FromSeconds(1);
    cfg.PermitLimit = 100;
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();


app.MapOpenApi();
app.MapScalarApiReference();
app.UseCors
    (x=>
    x.AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(t=>true)
    );

app.RegisterRoutes();

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed");

app.Run();
