using TenantGuard.API.Middleware;
using TenantGuard.API.Swagger;
using TenantGuard.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseMiddleware<TenantResolutionMiddleware>();
app.UseMiddleware<RequestLoggingScopeMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
