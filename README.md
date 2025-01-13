# ManagementSystem API

ManagementSystem API es una aplicación para gestionar reservas de salas, usuarios y otros recursos. Este proyecto está construido con ASP.NET Core y utiliza varias bibliotecas y herramientas para proporcionar una API robusta y segura.

## Requisitos Previos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Node.js](https://nodejs.org/) (opcional, para herramientas de desarrollo adicionales)

## Configuración del Proyecto

### Clonar el Repositorio

```bash
git clone https://github.com/migueldeltorodev/CoworkersMS.git
cd management-system-api
```

### Configurar la base de datos

1. Crea una base de datos en SQL Server.
2. Actualiza la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tu-servidor;Database=tu-base-de-datos;User Id=tu-usuario;Password=tu-contraseña;"
}
```

### Configurar JWT

Actualiza tu configuracion de JWT en tu `appsettings.json`

```json
"JwtSettings": {
  "Secret": "tu-secreto",
  "ExpiryMinutes": 60,
  "Issuer": "tu-issuer",
  "Audience": "tu-audience"
}
```

### Configurar Serilog

Actualizar en `appsettings.json` la configuracion de Serilog

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Debug",
    "Override": {
      "Microsoft": "Warning",
      "Microsoft.AspNetCore.Hosting.Diagnostics": "Error",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "WriteTo": [
    {
      "Name": "File",
      "Args": {
        "path": "logs/log-.txt",
        "rollOnFileSizeLimit": true,
        "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
      }
    }
  ],
  "Enrich": [
    "WithMachineName",
    "WithProcessId",
    "WithThreadId"
  ]
}
```

### Migraciones de la base de datos

Ejecuta las migraciones para crear las tablas necesarias en la base de datos:

`dotnet ef database update`

### Ejecucion del proyecto

`dotnet run`

### Documentacion de la API

La documentacion de la API esta disponible en Swagger. Puedes acceder a ella con

```bash
https://localhost:5001/swagger/index.html
```

### Estructura del Proyecto

- Common: Contiene clases y utilidades comunes, como excepciones y comportamientos.
- Contracts: Define los DTOs y las solicitudes/respuestas de la API.
- Endpoints: Define los endpoints de la API.
- Features: Contiene la lógica de negocio organizada en comandos y consultas.
- Persistence: Contiene la configuración de la base de datos y los repositorios.
- Program.cs: Configuración principal de la aplicación.



