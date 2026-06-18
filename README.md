# OrderSystem

Sistema de gestión de pedidos con arquitectura limpia (Clean Architecture) implementado en .NET 9.

## Stack tecnológico

| Tecnología | Versión |
|---|---|
| .NET | 9.0 |
| ASP.NET Core Web API | 9.0 |
| Entity Framework Core | 9.0.5 |
| PostgreSQL | 16 |
| JWT (Bearer) | — |
| Swagger / OpenAPI | — |
| xUnit + Moq | — |
| BCrypt.Net | — |
| Docker | — |

## Arquitectura

El proyecto sigue los principios de **Clean Architecture** con 4 capas:

```
OrderSystem.Domain      → Entidades, interfaces de repositorio, value objects
OrderSystem.Application → DTOs, casos de uso (use cases)
OrderSystem.Infrastructure → Persistencia (EF Core), repositorios, servicios (JWT)
OrderSystem.API         → Controladores, middleware, configuración
```

### Capa Domain
Entidades del negocio: `User`, `Product`, `Order`, `OrderItem`. Contiene las interfaces de repositorio (`IProductRepository`, `IOrderRepository`, `IUserRepository`) y de servicios (`IJwtService`). No tiene dependencias externas.

### Capa Application
Casos de uso orquestan la lógica de negocio usando las interfaces del dominio. Contiene los DTOs para entrada/salida de datos.

### Capa Infrastructure
Implementa los repositorios con Entity Framework Core y PostgreSQL. Incluye el `AppDbContext`, las migraciones y el servicio de generación de JWT.

### Capa API
Controladores REST, configuración de JWT, CORS, Swagger, manejo global de excepciones y seed de datos iniciales.

## Endpoints

| Método | Ruta | Auth | Rol | Descripción |
|---|---|---|---|---|
| POST | `/api/auth/register` | ❌ | — | Registrar un nuevo usuario |
| POST | `/api/auth/login` | ❌ | — | Iniciar sesión (devuelve JWT) |
| GET | `/api/products` | ❌ | — | Listar todos los productos |
| POST | `/api/products` | ✅ | Admin | Crear un producto |
| PUT | `/api/products/{id}/stock` | ✅ | Admin | Actualizar stock de un producto |
| POST | `/api/orders` | ✅ | Cualquiera | Crear una orden |
| GET | `/api/orders/my` | ✅ | Cualquiera | Listar órdenes del usuario autenticado |

## Seed data

Al iniciar, la aplicación ejecuta las migraciones automáticamente y precarga:

**Usuario administrador:**
- Email: `admin@example.com`
- Password: `Admin123!`

**Productos iniciales:**
| Producto | Precio | Stock |
|---|---|---|
| Laptop Gamer | $1,499.99 | 20 |
| Auriculares Bluetooth | $129.50 | 15 |
| Teclado mecánico | $89.90 | 8 |

## Ejecutar con Docker

```bash
docker compose up -d --build
```

La API se levanta en `http://localhost:5108`.

Para ver los logs:
```bash
docker compose logs -f api
```

Para detener:
```bash
docker compose down
```

Para eliminar también los datos de la BD:
```bash
docker compose down -v
```

## Ejecutar sin Docker

### Requisitos
- .NET SDK 9.0
- PostgreSQL 16 corriendo en `localhost:5432`

### Configurar la base de datos
La conexión por defecto en `appsettings.json`:
```
Host=localhost;Database=ordersdb;Username=postgres;Password=fabian123
```

### Ejecutar
```bash
dotnet run --project OrderSystem.API
```

La API estará disponible en `http://localhost:5108` o `https://localhost:7024`.

### Ejecutar tests
```bash
dotnet test OrderSystem.Tests
```

## Variables de entorno (Docker)

| Variable | Descripción |
|---|---|
| `ConnectionStrings__DefaultConnection` | Cadena de conexión a PostgreSQL |
| `Jwt__Key` | Clave secreta para firmar tokens JWT |
| `Jwt__Issuer` | Emisor del JWT |
| `Jwt__Audience` | Audiencia del JWT |
| `ASPNETCORE_ENVIRONMENT` | Entorno (Development / Production) |

## Desarrollo

### Estructura de carpetas

```
OrderSystem/
├── OrderSystem.API/            # Controladores, Program.cs, configuración
│   ├── Controllers/
│   ├── Properties/
│   └── appsettings.json
├── OrderSystem.Application/    # Casos de uso y DTOs
│   ├── DTOs/
│   └── UseCases/
├── OrderSystem.Domain/         # Entidades e interfaces
│   ├── Entities/
│   ├── Interfaces/
│   └── ValueObjects/
├── OrderSystem.Infrastructure/ # EF Core, repositorios, servicios
│   ├── Migrations/
│   ├── Persistence/
│   ├── Repositories/
│   └── Services/
├── OrderSystem.Tests/          # Tests unitarios (xUnit + Moq)
├── docker-compose.yml
├── Dockerfile
└── OrderSystem.slnx
```
