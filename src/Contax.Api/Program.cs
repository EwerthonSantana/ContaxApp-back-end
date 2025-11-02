using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myVueAppOrigins";
var builder = WebApplication.CreateBuilder(args);
{


    // 1. Configurações da Aplicação e Infraestrutura
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // 2. Outros Serviços
    builder.Services.AddControllers(options =>
    {
        // NOVO: Adiciona o filtro para tratar a exceção de validação
        options.Filters.Add<ValidationExceptionFilter>();
    });
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

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              // ** MUITO IMPORTANTE: Defina a URL do seu frontend Vue **
                              policy.WithOrigins("http://localhost:5173")
                                    // Permite todos os métodos HTTP (GET, POST, PUT, DELETE, OPTIONS)
                                    .AllowAnyMethod()
                                    // Permite todos os cabeçalhos de requisição (necessário para Authorization)
                                    .AllowAnyHeader()
                                    // Permite o envio de credenciais (cookies, headers de autenticação, etc.)
                                    // ESSENCIAL para JWT/Tokens de autenticação
                                    .AllowCredentials();
                          });
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
    app.UseCors(MyAllowSpecificOrigins); // <-- AQUI!
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
