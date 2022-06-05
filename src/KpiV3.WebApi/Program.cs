using KpiV3.Domain;
using KpiV3.Infrastructure;
using KpiV3.WebApi.Authentication.Extensions;
using KpiV3.WebApi.Extensions;
using KpiV3.WebApi.HostedServices.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddHttpContextAccessor();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = false;
    o.DefaultApiVersion = new ApiVersion(3, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = new UrlSegmentApiVersionReader();
});
services.AddAuthorization(options =>
{
    options.AddPolicy("RootOnly",
        policy => policy.RequireClaim("posType", "Root"));
});

services
    .AddDomainServices()
    .AddAdapters(configuration, builder.Environment)
    .AddMediatR(DomainAssembly.Instance, InfrastructureAssembly.Instance)
    .AddJwt(configuration)
    .AddHostedServices(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program
{
}