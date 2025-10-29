# 🏥 RespiCare - Sistema Integral de Enfermedades Respiratorias

Sistema completo de gestión y análisis de enfermedades respiratorias con ASP.NET MVC, Python AI Services y MongoDB.

## 📁 Estructura del Proyecto

```
Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva/
│
├── AspNetMvc/                      # 🎯 Backend + Frontend (ASP.NET MVC)
│   ├── Controllers/                # Controladores MVC
│   ├── Models/                     # Modelos de datos MongoDB
│   ├── Views/                      # Vistas Razor
│   ├── Services/                   # Lógica de negocio
│   ├── Helpers/                    # Utilidades
│   ├── App_Start/                  # Configuración MVC
│   ├── bin/                        # Binarios compilados
│   ├── Web.config                  # Configuración principal
│   ├── Dockerfile                  # Docker para ASP.NET MVC
│   └── *.csproj, *.sln            # Archivos del proyecto
│
├── ai-services/                    # 🤖 Servicios de IA (Python FastAPI)
│   ├── src/                        # Código fuente Python
│   ├── models/                     # Modelos ML entrenados
│   ├── tests/                      # Tests unitarios
│   ├── main.py                     # Punto de entrada
│   ├── requirements.txt            # Dependencias Python
│   └── dockerfile                  # Docker para AI Services
│
├── mongodb/                        # 🗄️ Configuración MongoDB
│   └── init/                       # Scripts de inicialización
│       └── init-db.js             # Script inicial de DB
│
├── nginx/                          # 🌐 Reverse Proxy (Opcional)
│   ├── nginx.conf                  # Configuración nginx
│   └── ssl/                        # Certificados SSL
│
├── docs/                           # 📚 Documentación Técnica
│   ├── README_MIGRACION.md         # Guía de migración
│   ├── ESTRUCTURA_PROYECTO.md      # Estructura detallada
│   ├── RESUMEN_MIGRACION.md        # Resumen ejecutivo
│   └── INSTRUCCIONES_EJECUCION.md  # Instrucciones de uso
│
├── Documentation/                  # 📖 Documentación General
│   └── (documentos del proyecto)
│
├── docker-compose.yml              # 🐳 Docker Compose (Producción)
├── docker-compose.dev.yml          # 🐳 Docker Compose (Desarrollo)
├── .gitignore                      # Archivos ignorados por Git
└── README.md                       # Este archivo
```

## 🚀 Inicio Rápido

### Opción 1: Desarrollo Local (RECOMENDADO)

```bash
# 1. Iniciar servicios Docker (MongoDB + AI Services)
docker-compose -f docker-compose.dev.yml up -d

# 2. Abrir Visual Studio
# Abrir: AspNetMvc/Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.sln

# 3. Presionar F5 para ejecutar
```

### Opción 2: Todo con Docker

```bash
# Requiere Windows Containers
docker-compose up -d
```

## 🌐 URLs del Sistema

| Servicio | URL | Credenciales |
|----------|-----|--------------|
| **ASP.NET MVC** | https://localhost:44367/ | - |
| **Dashboard** | https://localhost:44367/DashboardRespiCare | - |
| **MongoDB Express** | http://localhost:8081 | admin / admin123 |
| **AI Services Docs** | http://localhost:8000/docs | - |
| **AI Health Check** | http://localhost:8000/api/v1/health | - |

## 📋 Componentes del Sistema

### 🎯 ASP.NET MVC (Backend + Frontend)

**Stack Tecnológico:**
- ASP.NET MVC 5 (.NET Framework 4.8)
- C# 7.3+
- Razor Views
- MongoDB Driver 2.19.0
- Bootstrap 5

**Características:**
- ✅ Arquitectura MVC limpia
- ✅ Servicios de negocio separados
- ✅ Integración con MongoDB
- ✅ Integración con AI Services
- ✅ Vistas responsive con Bootstrap

**Carpetas Principales:**
- `Controllers/` - Lógica de control
- `Models/` - Modelos de datos
- `Views/` - Vistas Razor
- `Services/` - Lógica de negocio
- `Helpers/` - Utilidades

### 🤖 AI Services (Python FastAPI)

**Stack Tecnológico:**
- Python 3.11+
- FastAPI
- XGBoost (99.81% accuracy)
- SHAP (Explicabilidad ML)
- scikit-learn

**Características:**
- ✅ API RESTful con FastAPI
- ✅ Análisis de síntomas con ML
- ✅ Predicciones de enfermedades
- ✅ Chatbot médico inteligente
- ✅ Explicabilidad con SHAP

**Endpoints Principales:**
- `POST /api/v1/analyze` - Analizar síntomas
- `POST /api/v1/ml/predict` - Predicción ML
- `POST /api/v1/chat/process` - Chatbot
- `GET /api/v1/health` - Health check

### 🗄️ MongoDB

**Configuración:**
- MongoDB 6.0
- Puerto: 27017
- Base de datos: `respicare`
- Usuario: `admin` / Contraseña: `password123`

**Colecciones:**
- `usuarios` - Usuarios del sistema
- `medicalhistories` - Historiales médicos
- `symptomreports` - Reportes de síntomas
- `aianalyses` - Análisis de IA
- `chatconversations` - Conversaciones chatbot

### 🌐 Nginx (Opcional)

**Función:**
- Reverse proxy
- Load balancing
- SSL/TLS termination

## 🛠️ Instalación

### Prerrequisitos

- **Docker Desktop** (con soporte Linux containers)
- **Visual Studio 2019/2022**
- **.NET Framework 4.8**
- **Git**

### Pasos de Instalación

1. **Clonar el repositorio:**
```bash
git clone <tu-repositorio>
cd Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva
```

2. **Instalar paquetes NuGet:**
```powershell
# En Visual Studio > Tools > NuGet Package Manager > Package Manager Console
Update-Package -reinstall
```

3. **Iniciar servicios Docker:**
```bash
docker-compose -f docker-compose.dev.yml up -d
```

4. **Verificar servicios:**
```bash
docker-compose -f docker-compose.dev.yml ps
```

5. **Abrir y ejecutar en Visual Studio:**
- Abrir `AspNetMvc/Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.sln`
- Presionar F5

## 📝 Configuración

### Web.config (ASP.NET MVC)

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

### Variables de Entorno (AI Services)

```env
DATABASE_URL=mongodb://admin:password123@mongodb:27017/respicare?authSource=admin
MODEL_PATH=/app/models
LOG_LEVEL=DEBUG
```

## 🔧 Comandos Útiles

### Docker

```bash
# Ver estado
docker-compose -f docker-compose.dev.yml ps

# Ver logs
docker-compose -f docker-compose.dev.yml logs -f

# Reiniciar servicios
docker-compose -f docker-compose.dev.yml restart

# Detener servicios
docker-compose -f docker-compose.dev.yml down

# Limpiar todo (incluyendo volúmenes)
docker-compose -f docker-compose.dev.yml down -v
```

### MongoDB

```bash
# Conectarse a MongoDB
docker exec -it respicare-mongodb-dev mongosh -u admin -p password123

# Backup
docker exec respicare-mongodb-dev mongodump --out /backup

# Restaurar
docker exec respicare-mongodb-dev mongorestore /backup
```

## 📚 Documentación

### Documentación Técnica (docs/)

- **`README_MIGRACION.md`** - Guía completa de migración (400+ líneas)
- **`ESTRUCTURA_PROYECTO.md`** - Estructura detallada del proyecto
- **`RESUMEN_MIGRACION.md`** - Resumen ejecutivo de la migración
- **`INSTRUCCIONES_EJECUCION.md`** - Instrucciones paso a paso

### Documentación General (Documentation/)

- Documentos del proyecto original
- Especificaciones técnicas
- Diagramas UML

## 🧪 Testing

```bash
# Tests de AI Services
cd ai-services
pytest tests/

# Tests de ASP.NET MVC
# En Visual Studio > Test > Run All Tests
```

## 🐛 Solución de Problemas

### MongoDB no conecta

```bash
# Reiniciar MongoDB
docker restart respicare-mongodb-dev

# Ver logs
docker logs respicare-mongodb-dev
```

### AI Services no responde

```bash
# Reiniciar AI Services
docker restart respicare-ai-dev

# Ver logs
docker logs respicare-ai-dev -f
```

### Paquetes NuGet fallan

```powershell
# Limpiar cache
dotnet nuget locals all --clear

# Restaurar
Update-Package -reinstall
```

## 🔒 Seguridad

- ✅ Autenticación con hash SHA256
- ✅ Validación de entrada en todos los endpoints
- ✅ MongoDB con autenticación
- ✅ HTTPS en producción
- ✅ CORS configurado

## 📊 Estadísticas

- **Modelos de Datos**: 8 modelos principales
- **Servicios**: 4 servicios de negocio
- **Controladores**: 4 controladores MVC
- **Vistas Razor**: 3+ vistas creadas
- **Líneas de Código**: ~3,600 líneas C#
- **Cobertura ML**: 124 enfermedades respiratorias
- **Precisión ML**: 99.81% con XGBoost

## 🤝 Contribución

1. Fork el proyecto
2. Crear rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

## 📄 Licencia

MIT License

## 👥 Equipo

Equipo de desarrollo RespiCare

## 📞 Soporte

- 📧 Email: support@respicare.com
- 🐛 Issues: GitHub Issues
- 📚 Docs: `/docs` directory

---

<div align="center">
  <strong>🏥 RespiCare © 2025 - Cuidando tu salud respiratoria con tecnología avanzada</strong>
  <br><br>
  <img src="https://img.shields.io/badge/ASP.NET-MVC-blue" alt="ASP.NET MVC">
  <img src="https://img.shields.io/badge/Python-FastAPI-green" alt="Python FastAPI">
  <img src="https://img.shields.io/badge/MongoDB-6.0-green" alt="MongoDB">
  <img src="https://img.shields.io/badge/Docker-Enabled-blue" alt="Docker">
  <img src="https://img.shields.io/badge/ML-99.81%25-success" alt="ML Accuracy">
</div>

