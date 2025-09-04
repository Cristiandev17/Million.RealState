using Microsoft.EntityFrameworkCore;
using Million.RealState.Infrastructure.Data;
using Million.RealState.Application.Extensions;
using Million.RealState.Domain.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.BaseRegister(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RealStateDbContext>();
    dbContext.Database.Migrate();
    DatabaseInitializer.Initialize(dbContext);
}

app.UseCors(Constants.MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
