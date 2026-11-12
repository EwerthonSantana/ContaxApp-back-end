# ⚙️ Contax-API | Contact Management System

**(.NET 9 Web API with DDD, CQRS, and Dapper)**

> Robust back end for the contact management system. The architecture uses the **CQRS** pattern for separation of concerns and **Dapper** for optimized, high-performance queries.

---

## 🏗️ Architecture & Technologies

This project follows **Domain-Driven Design (DDD)**, with a **CQRS (Command Query Responsibility Segregation)** implementation and a strong separation of concerns across layers:

### Main Patterns and Libraries

| Category          | Technology / Pattern      | Usage                                                                                    |
| :---------------- | :------------------------ | :--------------------------------------------------------------------------------------- |
| **Architecture**  | **CQRS**                  | Logical separation between Commands (writes/mutations) and Queries (reads).              |
| **Write ORM**     | **Entity Framework Core** | Used for Command (write) operations and migration management.                            |
| **Read ORM**      | **Dapper**                | Used for high-performance Query (read) operations and direct SQL queries.                |
| **Mapping**       | **AutoMapper**            | Automated mapping between Entities, DTOs, and ViewModels.                                |
| **Validation**    | **FluentValidation**      | Clear, separate validation rules for Commands and Queries.                               |
| **Documentation** | **Swagger/OpenAPI**       | Interactive documentation and testing of the API endpoints.                              |
| **Database**      | PostgreSQL                | Transactional data storage.                                                              |

### Project Layers (DDD)

- **Contax.Domain** | Entities, Aggregates, Repositories, and Business Rules.
- **Contax.Application** | Application Logic, **Commands** (Create, Update), **Queries** (Read), Handlers, and DTOs.
- **Contax.Infrastructure** | EF Core implementation, Repositories, Dapper connection, and infrastructure services.
- **Contax.Api** | Entry point, Controllers, Dependency Injection setup, and CORS.

---

## 🔑 Setup and Running (with Docker)

The project is orchestrated with Docker Compose, which starts the API (.NET) and PostgreSQL and applies the EF Core migrations on startup.

### Prerequisites

- **Docker** and **Docker Compose**
- **Shared network:** create the global network used to communicate with the front end: `docker network create minha_rede_global`

### 1. Start the Back End and the Database

1.  Go to this project's directory (`Contax-Api`).
2.  Run the command to start all services:

    ```bash
    docker-compose up -d --build
    ```

    > **Note:** EF Core migrations and the Dapper setup run when the API container starts.

### Access and Verification

- **Documentation (Swagger):** the main entry point for testing commands and queries.
  - URL: **[http://localhost:8081/swagger](http://localhost:8081/swagger)**
- **Back-end status:** the API listens on `localhost:8081`.

---

## 🛠️ CQRS Implementation Details

The Application layer (`Contax.Application`) is structured to handle:

- **Commands (`Commands`):** processed by Handlers that use **EF Core** for transactional persistence.
- **Queries (`Queries`):** processed by Handlers that use **Dapper** to run optimized SQL queries directly against the database, returning read-specific DTOs.

### Environment Variables

Database and security settings are injected via `docker-compose.yml`.

| Variable                               | Value                   | Usage                                           |
| :------------------------------------- | :---------------------- | :---------------------------------------------- |
| `ConnectionStrings__DefaultConnection` | `Host=db;...`           | Main connection (used by EF Core).              |
| `ConnectionStrings__DapperConnection`  | `Host=db;...`           | Connection for Queries (used by Dapper).        |
| `Jwt__Key`                             | `<your-secret-key>`     | Secret key used to sign and validate JWTs.      |

---

## 📄 License

This project is licensed under the MIT License.

---

**Author:** Ewerthon Santana · [GitHub](https://github.com/EwerthonSantana) · [LinkedIn](https://www.linkedin.com/in/ewerthonsantana/)
