using System.Data;
using Contax.Domain.Interfaces;
using Contax.Infrastructure.Persistence;
using Contax.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // EF Core Setup (Configuração de conexão deve estar em appsettings.json da API)
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );
        // Injeção da Conexão Dapper
        services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(
            configuration.GetConnectionString("DefaultConnection")
        ));

        // Injeção do Repositório (SOLID - OCP/DIP)
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Opcional: Injeção do RabbitMQ
        // services.AddSingleton<IMessageProducer, RabbitMqProducer>();

        return services;
    }
}
