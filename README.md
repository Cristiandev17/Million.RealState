# Million.RealState.Api

API RESTful para la gestión de propiedades inmobiliarias.

## Características principales

- **Arquitectura limpia:** Separación en capas (Api, Application, Domain, Infrastructure) siguiendo buenas prácticas de Clean Architecture.
- **Patrones CQRS y Repository:** Implementación de Command Query Responsibility Segregation (CQRS) y patrón Repository para la gestión de datos.
- **Autenticación JWT:** Seguridad mediante autenticación por token JWT.
- **Modelo de maduración de Richardson:** La API sigue el modelo de maduración de Richardson para APIs RESTful.
- **Inyección de dependencias:** Uso de DI para desacoplar componentes y facilitar pruebas.
- **Automapper:** Mapeo automático entre entidades y DTOs.
- **Base de datos SQL Server:** Persistencia con Entity Framework Core en modo Code First.
- **Pruebas unitarias:** Tests sobre los handlers usando NUnit y Moq.

## Requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server

## Configuracion BD

1. Configura la cadena de conexión en [`Million.RealState.Api/appsettings.json`](Million.RealState.Api/appsettings.json).

## Ejecución

1. Ejecuta la API:
   ```sh
   dotnet run --project Million.RealState.Api/Million.RealState.Api.csproj
   ```

## Pruebas

Ejecuta las pruebas unitarias con:

```sh
dotnet test Million.RealState.Test/Million.RealState.Test.csproj
```

## Dependencias principales

- MediatR
- Entity Framework Core
- FluentValidation
- AutoMapper
- JWT Bearer Authentication
- NUnit, Moq


## Uso de la API

Para consumir los endpoints protegidos, primero debes obtener un token JWT de autenticación.

### 1. Obtener Token

Realiza una petición POST al endpoint de autenticación (por ejemplo, `/api/auth/login`) enviando un objeto JSON con las siguientes condiciones:

- **userName:** Debe contener la palabra `Million` (por ejemplo, `Juan.Million`).
- **Password:** Debe tener exactamente 5 caracteres, incluyendo al menos:
  - 1 letra mayúscula
  - 1 número
  - El resto letras minúsculas

#### Ejemplo de petición

- /api/v1/authentications/login

```json
{
    "userName": "Juan.Million",
    "Password": "Abcd5"
}
```

Si las credenciales cumplen las condiciones, recibirás un token JWT en la respuesta.

### 2. Consumir Endpoints Protegidos

Incluye el token en el encabezado `Authorization` de tus peticiones:

Ahora puedes acceder a los endpoints expuestos por la API.


