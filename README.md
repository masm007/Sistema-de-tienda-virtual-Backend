# 🛒 Tienda Virtual API

API desarrollada en .NET 9 usando arquitectura limpia (Clean Architecture).

## 🚀 Tecnologías
- ASP.NET Core Minimal API
- Entity Framework Core
- MySQL
- JWT Authentication

## 📂 Estructura del proyecto
- **Application** → Casos de uso, DTOs, interfaces
- **Domain** → Entidades y contratos
- **Data** → Acceso a datos (repositorios, DbContext)
- **API** → Endpoints y configuración

## 🔐 Autenticación
Se utiliza JWT para autenticación de usuarios.

## ⚙️ Configuración

Crear un archivo `appsettings.json` o usar variables de entorno:

en caso de usar .env debe ser asi:
ConnectionStrings__DefaultConnection=Server=tu_servidor;Port=tu_puerto;Database=tu_bd;User=root;Password=;
Jwt__Key=TU_CLAVE_DE_32_CARACTERES

en caso de usar todo en appsettings:
```json
"Jwt": {
  "Key": "REEMPLAZAR_CON_LA_CLAVE_QUE_QUIERAS",
  "Issuer": "TiendaApi",
  "Audience": "TiendaApiUsers",
  "ExpireMinutes": 30
}