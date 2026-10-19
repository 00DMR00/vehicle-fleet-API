# API de gestión de flota y alquiler de vehículos

API REST desarrollada con .NET 9 y arquitectura hexagonal. Permite registrar vehículos, consultar los que están disponibles y alquilarlos. MongoDB almacena los vehículos y alquileres.

## Requisitos

- [.NET SDK 9.0.200](https://dotnet.microsoft.com/download/dotnet/9.0) (la versión está fijada en `global.json`).
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) con Docker Compose, para ejecutar MongoDB localmente.
- Git, para clonar el repositorio.

## Descargar y ejecutar

Clona el repositorio y entra en su carpeta:

```bash
git clone <URL_DEL_REPOSITORIO>
cd <CARPETA_DEL_REPOSITORIO>
```

Inicia MongoDB desde la raíz del proyecto:

```bash
docker compose up -d mongodb
```

Restaura los paquetes y ejecuta la API en modo desarrollo:

```bash
dotnet restore src/microservice.sln
ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:8080 dotnet run --project src/GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj
```

En PowerShell, configura el entorno antes de ejecutar el proyecto:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_URLS = "http://localhost:8080"
dotnet run --project src/GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj
```

La configuración de desarrollo conecta a MongoDB en `localhost:27017` y crea/utiliza la base de datos `GtMotive`. La documentación interactiva estará en [http://localhost:8080/swagger](http://localhost:8080/swagger).

Para detener MongoDB:

```bash
docker compose down
```

Los datos se conservan en el volumen `mongodb_data`. Para eliminarlos también, ejecuta `docker compose down -v`.

## Endpoints

### Listar vehículos disponibles

```http
GET /api/vehicles
```

Devuelve `200 OK` con una lista de vehículos no alquilados. Si no hay vehículos disponibles, la lista está vacía.

### Crear un vehículo

```http
POST /api/vehicles
Content-Type: application/json
```

```json
{
  "licensePlate": "1234ABC",
  "brand": "Seat",
  "model": "Ibiza",
  "manufacturingDate": "2025-01-15"
}
```

Devuelve `201 Created`. La matrícula debe ser única y el vehículo debe cumplir la regla de antigüedad del dominio.

### Alquilar un vehículo

```http
POST /api/rentals
Content-Type: application/json
```

```json
{
  "vehicleId": "<id-del-vehiculo>",
  "customerId": "cliente-123"
}
```

Devuelve `201 Created` si el alquiler se completa. Devuelve `404 Not Found` si no existe el vehículo y `409 Conflict` si el cliente ya tiene un alquiler activo o el vehículo no se puede alquilar.

## Ejecutar las pruebas

Desde la raíz del repositorio:

```bash
dotnet test src/microservice.sln
```

También puedes ejecutar por separado los proyectos unitario, de infraestructura y funcional:

```bash
dotnet test test/unit/GtMotive.Estimate.Microservice.UnitTests/GtMotive.Estimate.Microservice.UnitTests.csproj
dotnet test test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/GtMotive.Estimate.Microservice.InfrastructureTests.csproj
dotnet test test/functional/GtMotive.Estimate.Microservice.FunctionalTests/GtMotive.Estimate.Microservice.FunctionalTests.csproj
```

## Estructura

```text
src/
  GtMotive.Estimate.Microservice.Domain/            Modelo y reglas de negocio
  GtMotive.Estimate.Microservice.ApplicationCore/   Casos de uso y puertos
  GtMotive.Estimate.Microservice.Infrastructure/    Persistencia MongoDB
  GtMotive.Estimate.Microservice.Api/               Controladores y adaptadores HTTP
  GtMotive.Estimate.Microservice.Host/              Arranque y configuración de la aplicación
test/
  unit/                                             Pruebas unitarias
  infrastructure/                                   Pruebas de integración HTTP
  functional/                                       Pruebas funcionales
```

## Configuración

Los valores de desarrollo están en `src/GtMotive.Estimate.Microservice.Host/appsettings.Development.json`. Para cambiar la conexión o el nombre de la base de datos, configura las variables de entorno `MongoDb__ConnectionString` y `MongoDb__MongoDbDatabaseName`.

Fuera del entorno `Development`, el host carga configuración y secretos desde Azure Key Vault. Para ejecutar localmente, usa el entorno `Development` como se muestra arriba.
