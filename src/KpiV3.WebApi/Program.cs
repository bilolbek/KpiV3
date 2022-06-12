using KpiV3.Domain;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.WebApi.Authentication.DataContracts;
using KpiV3.WebApi.Extensions;
using KpiV3.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddControllers(options =>
{
    options.Filters.Add<ExceptionToResponseFilterAttribute>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddKpiV3Services(configuration, environment);

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
