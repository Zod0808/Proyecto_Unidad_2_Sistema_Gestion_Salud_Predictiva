# 🏥 RespiCare - Migración a ASP.NET MVC

## 📋 Descripción

Este proyecto es la migración del sistema **RespiCare** de React + Node.js a **ASP.NET MVC** con C#. Mantiene todas las funcionalidades del sistema original incluyendo integración con MongoDB y servicios de IA en Python.

## 🏗️ Arquitectura del Sistema

```
┌─────────────────────────────────────────────────────────────┐
│                    ASP.NET MVC (C#)                          │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │Controllers │  │  Services  │  │   Models   │             │
│  │            │──│            │──│            │             │
│  │  - CRUD    │  │  - Logic   │  │  - Entidades            │
│  │  - Views   │  │  - Data    │  │  - ViewModels           │
│  └────────────┘  └────────────┘  └────────────┘            │
└─────────────────────────────────────────────────────────────┘
         │                   │                   │
         ├───────────────────┼───────────────────┤
         ▼                   ▼                   ▼
    ┌─────────┐         ┌─────────┐      ┌──────────────┐
    │ MongoDB │         │   AI    │      │ Servicios IA │
    │ (Docker)│         │Services │      │   Python     │
    │         │         │ (Docker)│      │  FastAPI     │
    └─────────┘         └─────────┘      └──────────────┘
```

## 🚀 Inicio Rápido

### Prerrequisitos

- **Visual Studio 2019/2022** con soporte para ASP.NET
- **.NET Framework 4.8**
- **Docker Desktop** (para MongoDB y AI Services)
- **IIS Express** o **IIS** instalado

### Instalación

#### 1. Instalar Paquetes NuGet

Abrir la consola de NuGet Package Manager y ejecutar:

```powershell
Install-Package MongoDB.Driver -Version 2.19.0
Install-Package MongoDB.Bson -Version 2.19.0
Install-Package Newtonsoft.Json -Version 13.0.3
```

O restaurar todos los paquetes:

```powershell
Update-Package -reinstall
```

#### 2. Iniciar MongoDB y AI Services con Docker

```bash
# Opción 1: Solo MongoDB y AI Services (RECOMENDADO para desarrollo)
docker-compose -f docker-compose.dev.yml up -d

# Opción 2: Sistema completo con ASP.NET MVC en Docker
# (Requiere Windows Containers)
docker-compose up -d
```

#### 3. Configurar Cadenas de Conexión

Editar `Web.config` según tu entorno:

```xml
<connectionStrings>
  <add name="MongoDB" 
       connectionString="mongodb://admin:password123@localhost:27017/respicare?authSource=admin"/>
</connectionStrings>

<appSettings>
  <add key="MongoDBName" value="respicare"/>
  <add key="AIServiceUrl" value="http://localhost:8000"/>
</appSettings>
```

#### 4. Ejecutar la Aplicación

- Presionar **F5** en Visual Studio
- O hacer clic derecho en el proyecto → **Debug → Start New Instance**
- La aplicación se abrirá en: `https://localhost:44367/`

## 📁 Estructura del Proyecto

```
Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva/
├── Controllers/                    # Controladores MVC
│   ├── UsuarioController.cs
│   ├── HistorialMedicoRespiCareController.cs
│   ├── ReporteSintomasController.cs
│   └── DashboardRespiCareController.cs
│
├── Models/                         # Modelos de datos
│   ├── Usuario.cs
│   ├── HistorialMedicoRespiCare.cs
│   ├── ReporteSintomas.cs
│   ├── AnalisisIA.cs
│   ├── Sintoma.cs
│   ├── Ubicacion.cs
│   └── ConversacionChat.cs
│
├── Services/                       # Servicios de negocio
│   ├── UsuarioService.cs
│   ├── HistorialMedicoService.cs
│   ├── ReporteSintomasService.cs
│   └── AIService.cs
│
├── Helpers/                        # Helpers y utilidades
│   ├── MongoDBHelper.cs
│   └── HttpClientHelper.cs
│
├── Views/                          # Vistas Razor
│   ├── Usuario/
│   ├── HistorialMedicoRespiCare/
│   ├── ReporteSintomas/
│   └── DashboardRespiCare/
│
├── docker-compose.yml              # Docker para producción
├── docker-compose.dev.yml          # Docker para desarrollo
├── Dockerfile                      # Dockerfile para ASP.NET MVC
└── Web.config                      # Configuración de la aplicación
```

## 🔧 Características Migradas

### ✅ Backend (ASP.NET MVC + C#)

- ✅ **Modelos de Datos**
  - Usuario con roles (Patient, Doctor, Admin)
  - HistorialMedicoRespiCare con síntomas
  - ReporteSintomas con análisis de IA
  - AnalisisIA con diagnósticos posibles
  - ConversacionChat para chatbot médico

- ✅ **Servicios**
  - UsuarioService: CRUD y autenticación
  - HistorialMedicoService: Gestión de historiales
  - ReporteSintomasService: Reportes con análisis IA
  - AIService: Integración con Python FastAPI

- ✅ **Controladores**
  - UsuarioController
  - HistorialMedicoRespiCareController
  - ReporteSintomasController
  - DashboardRespiCareController

- ✅ **Integración con MongoDB**
  - Driver oficial de MongoDB para C#
  - Queries async/await
  - Índices y optimizaciones

- ✅ **Integración con AI Services (Python)**
  - HttpClient para llamadas a API FastAPI
  - Análisis de síntomas
  - Predicciones ML
  - Chatbot médico

### 🐳 Docker

- ✅ **Docker Compose para Desarrollo** (`docker-compose.dev.yml`)
  - MongoDB 6.0
  - Mongo Express (UI administración)
  - AI Services (Python FastAPI)
  
- ✅ **Docker Compose para Producción** (`docker-compose.yml`)
  - Incluye ASP.NET MVC (requiere Windows Containers)
  - MongoDB
  - AI Services
  - Networking configurado

## 📊 Base de Datos MongoDB

### Colecciones Principales

1. **usuarios**
   - Autenticación y roles
   - Información de perfil

2. **medicalhistories**
   - Historiales médicos
   - Síntomas y diagnósticos
   - Ubicación geográfica

3. **symptomreports**
   - Reportes de síntomas
   - Análisis de IA integrado
   - Estados (Pending, InReview, Reviewed, Closed)

4. **aianalyses**
   - Análisis de IA
   - Diagnósticos posibles
   - Nivel de urgencia y confianza

5. **chatconversations**
   - Conversaciones con chatbot
   - Historial de mensajes

### Acceso a MongoDB

- **Mongo Express UI**: http://localhost:8081
- **Usuario**: admin
- **Contraseña**: admin123

## 🤖 Servicios de IA

Los servicios de IA se ejecutan en Python con FastAPI y están disponibles en:

- **URL**: http://localhost:8000
- **Documentación API**: http://localhost:8000/docs
- **Health Check**: http://localhost:8000/api/v1/health

### Endpoints Principales

- `POST /api/v1/analyze` - Analizar síntomas
- `POST /api/v1/ml/predict` - Predicción ML de enfermedades
- `POST /api/v1/chat/process` - Procesar consulta de chatbot
- `GET /api/v1/health/detailed` - Estado detallado del servicio

## 🎯 Funcionalidades Principales

### 1. Dashboard RespiCare
- Estadísticas generales del sistema
- Métricas de usuarios, historiales y reportes
- Estado del servicio de IA

### 2. Gestión de Usuarios
- CRUD completo
- Roles: Patient, Doctor, Admin
- Autenticación y permisos

### 3. Historiales Médicos
- Registro de historiales con síntomas
- Búsqueda y filtros
- Estadísticas por paciente/doctor

### 4. Reportes de Síntomas
- Creación de reportes
- Análisis automático con IA
- Estados y seguimiento
- Mapa geográfico de casos

### 5. Analytics
- Tendencias temporales
- Síntomas más comunes
- Diagnósticos frecuentes
- Mapa de calor interactivo

## 🚧 Pendientes (Para Implementar Vistas)

### Vistas Razor Pendientes

Las siguientes vistas necesitan ser creadas basándose en las vistas de React:

1. **Dashboard/**
   - Index.cshtml - Dashboard principal
   - Analytics.cshtml - Analytics avanzados
   - MapaInteractivo.cshtml - Mapa de casos
   - Estadisticas.cshtml - Estadísticas generales

2. **Usuario/**
   - Index.cshtml - Lista de usuarios
   - Details.cshtml - Detalle de usuario
   - Create.cshtml - Crear usuario
   - Edit.cshtml - Editar usuario
   - Delete.cshtml - Confirmar eliminación

3. **HistorialMedicoRespiCare/**
   - Index.cshtml - Lista de historiales
   - Details.cshtml - Detalle de historial
   - Create.cshtml - Crear historial
   - Edit.cshtml - Editar historial
   - Urgentes.cshtml - Historiales urgentes

4. **ReporteSintomas/**
   - Index.cshtml - Lista de reportes
   - Details.cshtml - Detalle con análisis IA
   - Create.cshtml - Formulario de reporte
   - Mapa.cshtml - Visualización en mapa
   - Urgentes.cshtml - Reportes urgentes

## 📝 Comandos Útiles

### Docker

```bash
# Iniciar servicios de desarrollo
docker-compose -f docker-compose.dev.yml up -d

# Ver logs
docker-compose -f docker-compose.dev.yml logs -f

# Detener servicios
docker-compose -f docker-compose.dev.yml down

# Ver estado de contenedores
docker-compose -f docker-compose.dev.yml ps

# Reiniciar servicios
docker-compose -f docker-compose.dev.yml restart
```

### NuGet

```powershell
# Restaurar paquetes
Update-Package -reinstall

# Actualizar paquete específico
Update-Package MongoDB.Driver

# Listar paquetes instalados
Get-Package
```

### IIS

```powershell
# Reiniciar IIS
iisreset

# Listar sitios
Get-IISSite

# Iniciar/Detener sitio
Start-IISSite "Default Web Site"
Stop-IISSite "Default Web Site"
```

## 🔒 Seguridad

- **Autenticación**: Sistema de usuarios con hash de contraseñas (SHA256)
- **Roles**: Control de acceso basado en roles (RBAC)
- **Validación**: Validación de modelos con DataAnnotations
- **MongoDB**: Conexión con autenticación
- **HTTPS**: Configurado en IIS Express

## 🐛 Solución de Problemas

### Error: No se puede conectar a MongoDB

```bash
# Verificar que MongoDB esté ejecutándose
docker ps

# Ver logs de MongoDB
docker logs respicare-mongodb-dev

# Reiniciar MongoDB
docker restart respicare-mongodb-dev
```

### Error: No se puede conectar al servicio de IA

```bash
# Verificar estado del servicio
docker logs respicare-ai-dev

# Probar endpoint manualmente
curl http://localhost:8000/api/v1/health

# Reiniciar servicio
docker restart respicare-ai-dev
```

### Error: Paquetes NuGet no se restauran

```powershell
# Limpiar cache de NuGet
dotnet nuget locals all --clear

# Restaurar paquetes
Update-Package -reinstall
```

## 📚 Recursos Adicionales

- [Documentación MongoDB C# Driver](https://www.mongodb.com/docs/drivers/csharp/)
- [ASP.NET MVC 5](https://docs.microsoft.com/en-us/aspnet/mvc/mvc5)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [FastAPI Documentation](https://fastapi.tiangolo.com/)

## 👥 Equipo

Equipo de desarrollo RespiCare

## 📄 Licencia

MIT License

---

**RespiCare** - Sistema Integral de Enfermedades Respiratorias 🏥✨

