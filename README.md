# ⚙️ Contax-API | Sistema de Gestão de Contatos

**(Backend .NET 9 Web API em DDD, CQRS e Dapper)**

> Aplicação backend robusta para o sistema de gestão de contatos. Esta arquitetura utiliza o padrão **CQRS** para separação de responsabilidades e **Dapper** para consultas otimizadas de alto desempenho.

---

## 🏗️ Arquitetura & Tecnologias

Este projeto segue o padrão **Domain-Driven Design (DDD)**, com uma implementação de **CQRS (Command Query Responsibility Segregation)** e uma forte separação de responsabilidades nas camadas:

### Padrões e Bibliotecas Principais

| Categoria          | Tecnologia / Padrão       | Uso                                                                                     |
| :----------------- | :------------------------ | :-------------------------------------------------------------------------------------- |
| **Arquitetura**    | **CQRS**                  | Separação lógica entre Comandos (escrita/mutação) e Queries (leitura/consulta).         |
| **ORM de Escrita** | **Entity Framework Core** | Utilizado para operações de Comando (escrita) e gestão de Migrations.                   |
| **ORM de Leitura** | **Dapper**                | Utilizado para operações de Query (leitura) de alto desempenho e consultas SQL diretas. |
| **Mapeamento**     | **AutoMapper**            | Mapeamento automatizado entre Entidades, DTOs e ViewModels.                             |
| **Validação**      | **FluentValidation**      | Definição de regras de validação claras e separadas para Comandos e Queries.            |
| **Documentação**   | **Swagger/OpenAPI**       | Documentação e testes interativos dos endpoints da API.                                 |
| **Banco de Dados** | PostgreSQL                | Armazenamento de dados transacionais.                                                   |

### Camadas do Projeto (DDD)

- **Contax.Domain** | Contém Entidades, Agregados, Repositórios e Regras de Negócio.
- **Contax.Application** | Contém Lógica de Aplicação, **Comandos** (Criação, Edição), **Queries** (Consulta), Handlers e DTOs.
- **Contax.Infrastructure** | Implementação do EF Core, Repositórios, Conexão Dapper e serviços de infraestrutura.
- **Contax.Api** | Ponto de entrada, Controllers, Configuração de Injeção de Dependência e CORS.

---

## 🔑 Configuração e Execução (Com Docker)

O projeto é orquestrado via Docker Compose, que inicializa a API (.NET), o PostgreSQL e aplica as Migrações do EF Core no startup.

### Pré-Requisitos

- **Docker** e **Docker Compose**
- **Rede Compartilhada:** Crie a rede global para comunicação com o Frontend: `docker network create minha_rede_global`

### 1. Subir o Backend e o Banco de Dados

1.  Navegue até o diretório deste projeto (`Contax-Api`).
2.  Execute o comando para subir todos os serviços:

    ```bash
    docker-compose up -d --build
    ```

    > **Nota:** As Migrações do EF Core e a injeção do Dapper são configuradas para ocorrer no startup do contêiner da API.

### Acesso e Verificação

- **Documentação (Swagger):** O principal ponto de acesso para testar seus comandos e queries.
  - URL: **[http://localhost:8081/swagger](http://localhost:8081/swagger)**
- **Backend Status:** API escutando em `localhost:8081`.

---

## 🛠️ Detalhes da Implementação CQRS

A camada de Aplicação (`Contax.Application`) está estruturada para receber:

- **Comandos (`Commands`):** Tratados por Handlers que utilizam o **EF Core** para persistência transacional.
- **Queries (`Queries`):** Tratadas por Handlers que utilizam o **Dapper** para executar consultas SQL otimizadas diretamente no banco de dados, retornando DTOs específicos de leitura.

### Variáveis de Ambiente

As configurações de DB e segurança são injetadas via `docker-compose.yml`.

| Variável                               | Valor                              | Uso                                             |
| :------------------------------------- | :--------------------------------- | :---------------------------------------------- |
| `ConnectionStrings__DefaultConnection` | `Host=db;...`                      | Conexão principal (usada pelo EF Core).         |
| `ConnectionStrings__DapperConnection`  | `Host=db;...`                      | Conexão para as Queries (usada pelo Dapper).    |
| `Jwt__Key`                             | `dcd595086d344af278425b8c58eebae3` | Chave secreta para geração e validação de JWTs. |

---

## 📄 Licença

Este projeto está licenciado sob a Licença MIT.

---

**Autor:** Ewerthon Santana/https://github.com/EwerthonSantana/https://www.linkedin.com/in/ewerthonsantana/
