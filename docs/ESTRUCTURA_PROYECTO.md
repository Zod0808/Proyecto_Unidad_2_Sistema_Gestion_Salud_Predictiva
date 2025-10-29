# 📁 Estructura del Proyecto RespiCare ASP.NET MVC

## 🏗️ Arquitectura General

```
RespiCare/
│
├── Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva/  ← ASP.NET MVC (NUEVO)
│   ├── Controllers/                                           # Controladores MVC
│   ├── Models/                                                # Modelos de datos MongoDB
│   ├── Views/                                                 # Vistas Razor
│   ├── Services/                                              # Lógica de negocio
│   ├── Helpers/                                               # Utilidades
│   ├── docker-compose.yml                                     # Docker producción
│   ├── docker-compose.dev.yml                                 # Docker desarrollo
│   └── Web.config                                             # Configuración
│
└── proyecto-final-sistema_enfermedades_respiratorias/         
    ├── ai-services/                                           # Servicios IA (Python)
    ├── mobile/                                                # App móvil (React Native)
    ├── mongodb/                                               # Scripts MongoDB
    ├── nginx/                                                 # Nginx config
    └── Documentation/                                         # Documentación general
```

## 📂 Detalles de Carpetas Principales

### 🎮 Controllers/

Controladores que manejan las peticiones HTTP y coordinan entre vistas y servicios:

- **UsuarioController.cs** - Gestión de usuarios (CRUD)
- **HistorialMedicoRespiCareController.cs** - Historiales médicos
- **ReporteSintomasController.cs** - Reportes de síntomas con IA
- **DashboardRespiCareController.cs** - Dashboard y analytics
- **AuthsController.cs** - Autenticación (heredado)
- **PacienteController.cs** - Vista paciente (heredado)
- **MedicoController.cs** - Vista médico (heredado)

### 📊 Models/

Modelos de datos para MongoDB con atributos de validación:

**Modelos RespiCare (Nuevos):**
- **Usuario.cs** - Usuarios del sistema con roles
- **HistorialMedicoRespiCare.cs** - Historiales médicos completos
- **ReporteSintomas.cs** - Reportes de síntomas
- **AnalisisIA.cs** - Análisis de inteligencia artificial
- **Sintoma.cs** - Modelo de síntomas
- **Ubicacion.cs** - Ubicación geográfica
- **ConversacionChat.cs** - Conversaciones chatbot
- **DiagnosticoPosible.cs** - Diagnósticos posibles
- **MensajeChat.cs** - Mensajes de chat

**Modelos Heredados:**
- **Paciente.cs** - Pacientes (sistema antiguo)
- **Medico.cs** - Médicos (sistema antiguo)
- **Cita.cs** - Citas médicas
- **Especialidad.cs** - Especialidades médicas

### 🛠️ Services/

Capa de servicios con lógica de negocio y acceso a datos:

- **UsuarioService.cs** - Lógica de usuarios (CRUD, autenticación)
- **HistorialMedicoService.cs** - Gestión de historiales
- **ReporteSintomasService.cs** - Manejo de reportes
- **AIService.cs** - Integración con servicios IA (Python)

### 🔧 Helpers/

Utilidades y helpers del sistema:

- **MongoDBHelper.cs** - Conexión y gestión de MongoDB
- **HttpClientHelper.cs** - Cliente HTTP para servicios IA

### 🎨 Views/ (Pendiente - TODO)

Vistas Razor que deben ser creadas basándose en las vistas de React:

```
Views/
├── Usuario/
│   ├── Index.cshtml              # Lista de usuarios
│   ├── Details.cshtml            # Detalle de usuario
│   ├── Create.cshtml             # Crear usuario
│   ├── Edit.cshtml               # Editar usuario
│   └── Delete.cshtml             # Confirmar eliminación
│
├── HistorialMedicoRespiCare/
│   ├── Index.cshtml              # Lista de historiales
│   ├── Details.cshtml            # Detalle de historial
│   ├── Create.cshtml             # Crear historial con síntomas
│   ├── Edit.cshtml               # Editar historial
│   ├── Urgentes.cshtml           # Casos urgentes
│   ├── PorPaciente.cshtml        # Historiales por paciente
│   └── Estadisticas.cshtml       # Estadísticas
│
├── ReporteSintomas/
│   ├── Index.cshtml              # Lista de reportes
│   ├── Details.cshtml            # Detalle con análisis IA
│   ├── Create.cshtml             # Formulario de reporte
│   ├── Urgentes.cshtml           # Reportes urgentes
│   ├── PorEstado.cshtml          # Reportes por estado
│   ├── Mapa.cshtml               # Visualización en mapa
│   └── Estadisticas.cshtml       # Estadísticas de reportes
│
├── DashboardRespiCare/
│   ├── Index.cshtml              # Dashboard principal
│   ├── Analytics.cshtml          # Analytics avanzados
│   ├── MapaInteractivo.cshtml    # Mapa de calor
│   ├── Estadisticas.cshtml       # Estadísticas generales
│   ├── Urgencias.cshtml          # Panel de urgencias
│   └── TendenciasTemporal.cshtml # Tendencias en el tiempo
│
├── Share/
│   └── _Layout.cshtml            # Layout principal
│
└── _ViewStart.cshtml             # Vista de inicio
```

## 🔄 Flujo de Datos

```
Usuario
  ↓
Controller (ASP.NET MVC)
  ↓
Service (Lógica de negocio)
  ↓
MongoDB Driver ← → MongoDB (Docker)
  ↓
HttpClient → AI Services (Python/Docker)
  ↓
Controller
  ↓
View (Razor)
  ↓
Usuario
```

## 🗄️ Colecciones MongoDB

1. **usuarios** - Usuarios del sistema
2. **medicalhistories** - Historiales médicos
3. **symptomreports** - Reportes de síntomas
4. **aianalyses** - Análisis de IA
5. **chatconversations** - Conversaciones chatbot

## 🐳 Docker Services

### docker-compose.dev.yml (Desarrollo)

```yaml
services:
  - mongodb (puerto 27017)
  - mongo-express (puerto 8081)
  - ai-services (puerto 8000)
```

**ASP.NET MVC se ejecuta localmente en Visual Studio/IIS**

### docker-compose.yml (Producción)

```yaml
services:
  - mongodb
  - mongo-express
  - ai-services
  - aspnet-mvc (requiere Windows Containers)
```

## 🚀 Ejecución del Proyecto

### Desarrollo Local (RECOMENDADO)

1. **Iniciar servicios Docker**:
```bash
cd Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva
docker-compose -f docker-compose.dev.yml up -d
```

2. **Abrir en Visual Studio**:
   - Abrir `Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.sln`
   - Presionar F5 o hacer clic en "Start"

3. **Acceder a la aplicación**:
   - ASP.NET MVC: `https://localhost:44367/`
   - MongoDB Express: `http://localhost:8081`
   - AI Services: `http://localhost:8000/docs`

### Producción con Docker

```bash
cd Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva
docker-compose up -d
```

**Nota**: Requiere Docker con soporte para Windows Containers

## 📝 Configuración

### Web.config

```xml
<connectionStrings>
  <add name="MongoDB" 
       connectionString="mongodb://admin:password123@localhost:27017/respicare?authSource=admin"/>
</connectionStrings>

<appSettings>
  <add key="MongoDBName" value="respicare"/>
  <add key="AIServiceUrl" value="http://localhost:8000"/>
  <add key="JWTSecret" value="dev-secret-key-change-in-production"/>
</appSettings>
```

## 🔑 Credenciales por Defecto

### MongoDB
- **Usuario**: admin
- **Contraseña**: password123
- **Base de datos**: respicare

### Mongo Express UI
- **Usuario**: admin
- **Contraseña**: admin123
- **URL**: http://localhost:8081

## 📋 Estado de Migración

### ✅ Completado

- [x] Modelos de datos migrados a C#
- [x] Servicios de negocio implementados
- [x] Controladores MVC creados
- [x] Integración con MongoDB
- [x] Integración con AI Services (Python)
- [x] Docker Compose configurado
- [x] Documentación creada
- [x] Eliminación de carpetas React y Node.js

### ⏳ Pendiente

- [ ] Crear vistas Razor (Views)
- [ ] Implementar autenticación JWT
- [ ] Crear layout responsive
- [ ] Migrar componentes de React a Razor
- [ ] Implementar chatbot en vistas
- [ ] Crear mapa interactivo
- [ ] Implementar dashboard analytics

## 🎯 Próximos Pasos

1. **Crear vistas Razor basándose en componentes de React**
   - Usar Bootstrap/CSS del proyecto original
   - Implementar AJAX para llamadas asíncronas
   - Integrar Chart.js para gráficos

2. **Implementar autenticación**
   - Login/Register
   - JWT tokens
   - Sesiones

3. **Crear dashboard interactivo**
   - Gráficos con Chart.js
   - Mapa con Leaflet.js
   - Tablas con DataTables

4. **Testing**
   - Unit tests con NUnit
   - Integration tests
   - End-to-end tests

## 📚 Referencias

- [ASP.NET MVC 5 Documentation](https://docs.microsoft.com/en-us/aspnet/mvc/mvc5)
- [MongoDB C# Driver](https://www.mongodb.com/docs/drivers/csharp/)
- [Docker Documentation](https://docs.docker.com/)
- [Bootstrap 5](https://getbootstrap.com/)
- [Chart.js](https://www.chartjs.org/)

---

**RespiCare ASP.NET MVC** - Sistema Integral de Enfermedades Respiratorias 🏥

