using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region AddedByDeveloper

builder.LoadLogConfiguration();

IConfiguration _configuration = builder.Configuration;
builder.Services.LoadServiceMethods(_configuration);

builder.LoadHealthChecks();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region AddedByDeveloper

app.UseAuthentication();

#endregion

app.UseAuthorization();

#region AddedByDeveloper

app.UseSerilogRequestLogging();
app.UseMiddleware<WebApi.Middlewares.ExceptionHandlingMiddleware>();
app.UseMiddleware<WebApi.Middlewares.TokenManagerMiddleware>();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

#endregion

app.MapControllers();

app.Run();

