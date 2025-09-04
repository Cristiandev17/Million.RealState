using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens; 
using Million.RealState.Application.Mappers;
using Million.RealState.Domain.Interfaces.Repositories;
using Million.RealState.Domain.Utilities;
using Million.RealState.Infrastructure.Data;
using Million.RealState.Infrastructure.Repositories;
using System.Reflection;
using System.Reflection.Metadata;

namespace Million.RealState.Application.Extensions;

public static class DIExtensions
{
    //Method to register all base services for the application
    public static IServiceCollection BaseRegister(this IServiceCollection services, ConfigurationManager configuration)
    {
        return services
            .RegisterTools() // Register utility tools and frameworks
            .RegisterRepositories() // Register data repositories
            .RegisterAuthorization(configuration) // Configure authentication and authorization
            .RegisterContextDb(configuration); // Register database context

    }

    // Registers application tools and frameworks like AutoMapper and MediatR
    private static IServiceCollection RegisterTools(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PropertyImageMapperProfile), typeof(PropertyMapperProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    // Registers repository implementations with their interfaces
    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {       
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();

        return services;
    }

    // Configures authentication, authorization and CORS policies
    private static IServiceCollection RegisterAuthorization(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: Constants.MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:5173")
                                       .AllowAnyHeader()
                                       .AllowAnyMethod();
                              });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration[Constants.JWT],
                ValidAudience = configuration[Constants.JWTAudience],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configuration[Constants.JWTKey]))
            };

            options.UseSecurityTokenValidators = true;
        });

        return services;
    }

    // Registers the database context with SQL Server provider
    private static IServiceCollection RegisterContextDb(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<RealStateDbContext>(options =>
            options.UseSqlServer(configuration[Constants.Dbconnection]));
        return services;
    }
}