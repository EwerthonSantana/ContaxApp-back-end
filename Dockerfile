# Estágio de Build (SDK)
# Usa o SDK para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copia o arquivo de solução
COPY ContaxApp.sln .

# 2. Copia os arquivos .csproj de todos os projetos dependentes (para otimizar o restore)
COPY src/Contax.Api/Contax.Api.csproj src/Contax.Api/
COPY src/Contax.Application/Contax.Application.csproj src/Contax.Application/
COPY src/Contax.Domain/Contax.Domain.csproj src/Contax.Domain/
COPY src/Contax.Infrastructure/Contax.Infrastructure.csproj src/Contax.Infrastructure/
COPY src/Contax.Tests/Contax.Tests.csproj src/Contax.Tests/ 

# 3. Restaura as dependências (NuGet)
RUN dotnet restore ContaxApp.sln

# 4. Copia o restante do código-fonte
COPY src/ src/

# 5. Publica o projeto principal (API)
# O WORKDIR é opcional, mas vamos mantê-lo para referência. 
# O comando 'publish' usa o caminho relativo do projeto a partir da raiz /src.
WORKDIR /src
RUN dotnet publish src/Contax.Api/Contax.Api.csproj -c Release -o /app/publish

# Estágio Final (Runtime)
# Usa a imagem de runtime mais leve
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080 

# Copia os artefatos publicados do estágio de build
COPY --from=build /app/publish .

# Define o ponto de entrada para rodar a aplicação
ENTRYPOINT ["dotnet", "Contax.Api.dll"]