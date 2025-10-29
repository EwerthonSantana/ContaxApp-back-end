// Agenda.Api/Program.cs
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    // 1. Configurações da Aplicação e Infraestrutura
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // 2. Outros Serviços
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // 3. Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Agenda API", Version = "v1" });
        c.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
            }
        );
    });

    // Configuração JWT (Lê a chave secreta)
    var jwtKey =
        builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key não configurada em appsettings.json.");
    var key = Encoding.ASCII.GetBytes(jwtKey);

    builder
        .Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Ajustar para 'true' em produção
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Ajustar para 'true' em produção e configurar
                ValidateAudience = false, // Ajustar para 'true' em produção e configurar
                ValidateLifetime = true, // Valida a expiração do token
                ClockSkew = TimeSpan.Zero, // Não permite tolerância no tempo de expiração
            };
        });

    builder.Services.AddAuthorization();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
