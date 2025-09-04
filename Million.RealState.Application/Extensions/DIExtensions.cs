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
    public static IServiceCollection BaseRegister(this IServiceCollection services, ConfigurationManager configuration)
    {
        return services
            .RegisterTools()
            .RegisterRepositories()
            .RegisterAuthorization(configuration)
            .RegisterContextDb(configuration);

    }

    private static IServiceCollection RegisterTools(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PropertyImageMapperProfile), typeof(PropertyMapperProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {       
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();

        return services;
    }

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

    private static IServiceCollection RegisterContextDb(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<RealStateDbContext>(options =>
            options.UseSqlServer(configuration[Constants.Dbconnection]));
        return services;
    }
}