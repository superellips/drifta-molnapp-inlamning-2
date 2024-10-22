using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TipsRundan.Application.Interfaces;
using TipsRundan.Domain.Interfaces;
using TipsRundan.Infrastructure.DbContexts;
using TipsRundan.Infrastructure.Repositories;
using TipsRundan.Infrastructure.Security;

namespace TipsRundan.Infrastructure.Configuration;

public static class DependencyInjectionHelper
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfigurationManager configuration)
    {
        switch (configuration.GetSection("DbImplementation").Value)
        {
            case "SqliteLocal":
                var connection = new SqliteConnection(configuration.GetSection("SqliteConnectionString").Value);
                connection.Open();
                services.AddDbContext<SqliteDbContext>(optionsBuilder => 
                    optionsBuilder.UseSqlite(connection));
                services.AddScoped<IQuizRepository, QuizRepository>();
                break;
            case "MongoDb":
                var client = new MongoClient(configuration.GetSection("MongoDbConnection").Value);
                services.AddSingleton<IMongoClient>(client);
                services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("TipsRundan"));
                services.AddScoped<IAccountRepository, MongoAccountRepository>();
                services.AddScoped<IQuizRepository, MongoQuizRepository>();
                break;
            default:
                break;
        }

        services.AddScoped<IAuthnService, CookieAuthnService>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => 
                    {
                        options.LoginPath = "/User/Login";
                        options.LogoutPath = "/User/Logout";
                    });
        services.AddHttpContextAccessor();
        services.AddScoped<ICryptographyService, CryptographyService>();
        return services;
    }
}