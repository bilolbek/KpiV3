using KpiV3.Domain;
using KpiV3.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

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

services.AddAdapters(configuration, builder.Environment);

services.AddMediatR(DomainAssembly.Instance, InfrastructureAssembly.Instance);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program
{
}