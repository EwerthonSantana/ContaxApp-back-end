// Agenda.Infrastructure/DependencyInjection.cs
using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using Contax.Infrastructure.Persistence;
using Contax.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        // Injeção do Repositório (SOLID - OCP/DIP)
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Opcional: Injeção do RabbitMQ
        // services.AddSingleton<IMessageProducer, RabbitMqProducer>();

        return services;
    }
}
