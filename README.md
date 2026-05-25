# 🛒 Tienda Virtual API

API desarrollada en .NET 9 usando arquitectura limpia (Clean Architecture).

## 🚀 Tecnologías
- ASP.NET Core Minimal API
- Entity Framework Core
- MySQL
- JWT Authentication
- Cookies HttpOnly (para Refresh Tokens)

## 📂 Estructura del proyecto
- **Application** → Contiene la lógica de negocio: Casos de uso (UseCases), DTOs, Interfaces (contratos)
- **Domain** → Núcleo del sistema: Entidades, Reglas de negocio, Interfaces de repositorio
- **Data** → Acceso a datos: Implementaciones de repositorios, DbContext (EF Core), Configuración de base de datos
- **API** → Capa de presentación: Endpoints (Minimal API), Configuración de servicios, Autenticación y autorización

## 🔐 Autenticación
El sistema implementa un esquema de autenticación basado en:

- **JWT (Access Token)**
Se envía en cada request autenticado
Tiene corta duración
- **Refresh Token**
Generado de forma segura (RandomNumberGenerator)
Almacenado hasheado en base de datos
Enviado al cliente mediante cookie HttpOnly
Permite renovar el JWT sin volver a iniciar sesión

- **Flujo de autenticación**
- Login
Se valida usuario/contraseña
Se genera: JWT (response) y Refresh Token (cookie HttpOnly)
- Refresh
Se envía automáticamente la cookie
Se valida el token: No revocado, No expirado
Se genera un nuevo JWT + Refresh Token
Se revoca el anterior (rotación)
- Seguridad adicional
Detección de reuse attack
Posibilidad de invalidar todas las sesiones del usuario

## ⚙️ Configuración

Crear un archivo `appsettings.json` o usar variables de entorno:

en caso de usar .env debe ser asi:
ConnectionStrings__DefaultConnection=Server=tu_servidor;Port=tu_puerto;Database=tu_bd;User=root;Password=;
Jwt__Key=TU_CLAVE_DE_32_CARACTERES
FRONTEND_URL=http://host:puerto

en caso de usar todo en appsettings:
```json
"Jwt": {
  "Key": "REEMPLAZAR_CON_LA_CLAVE_QUE_QUIERAS",
  "Issuer": "TiendaApi",
  "Audience": "TiendaApiUsers",
  "ExpireMinutes": 30
}

Para ejecutar migraciones abrir la terminal y poner:
dotnet ef migrations add nombreActualizacion --project Data --startup-project TiendaVirtualApi
dotnet ef database update --project Data --startup-project TiendaVirtualApi