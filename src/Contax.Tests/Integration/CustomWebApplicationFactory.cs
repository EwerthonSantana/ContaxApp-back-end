using System;
using Contax.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove a Configuração do DbContext Real (PostgreSQL)
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>)
            );
            if (descriptor != null)
                services.Remove(descriptor);

            // 2. Adiciona o DbContext In-Memory para o Teste
            services.AddDbContext<AppDbContext>(options =>
            {
                // Nome do banco de dados em memória deve ser único por teste
                options.UseInMemoryDatabase("InMemoryDbForTesting_" + Guid.NewGuid().ToString());
            });

            // 3. Garante que o provedor de serviços seja criado
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                // Garante que o banco de dados está criado (e vazio)
                db.Database.EnsureCreated();

                // Opcional: Aqui você pode adicionar um SEED DATA de teste (ex: o usuário Admin para o JWT)
                // Exemplo: db.Users.Add(new User(...)); db.SaveChanges();
            }
        });
    }
}
