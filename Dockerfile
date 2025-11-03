# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura dependências
COPY *.sln .  
COPY src/Contax.Domain/Contax.Domain.csproj ./Contax.Domain/
COPY src/Contax.Application/Contax.Application.csproj ./Contax.Application/
COPY src/Contax.Infrastructure/Contax.Infrastructure.csproj ./Contax.Infrastructure/
COPY src/Contax.Api/Contax.Api.csproj ./Contax.Api/

RUN dotnet restore ./Contax.Api/Contax.Api.csproj

# Copia o restante do código
COPY src/ ./  
WORKDIR /src/Contax.Api

# Publica a aplicação
RUN dotnet publish Contax.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia apenas os arquivos da pasta publish
COPY --from=build /app/publish/ .

# Configura a entrada do container
ENTRYPOINT ["dotnet", "Contax.Api.dll"]
