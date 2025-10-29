# 🚀 Instrucciones de Ejecución - RespiCare ASP.NET MVC

## ⚡ Inicio Rápido (5 minutos)

### Paso 1: Instalar Paquetes NuGet

Abre Visual Studio 2019/2022 y la consola de NuGet Package Manager:

```powershell
# Opción 1: Restaurar todos los paquetes automáticamente
Update-Package -reinstall

# Opción 2: Instalar manualmente los paquetes principales
Install-Package MongoDB.Driver -Version 2.19.0
Install-Package MongoDB.Bson -Version 2.19.0
Install-Package Newtonsoft.Json -Version 13.0.3
Install-Package System.Buffers -Version 4.5.1
Install-Package System.Memory -Version 4.5.5
```

### Paso 2: Iniciar Servicios Docker

Abre PowerShell o CMD en la carpeta del proyecto:

```bash
# Navegar a la carpeta del proyecto
cd Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva

# Iniciar MongoDB y AI Services
docker-compose -f docker-compose.dev.yml up -d

# Verificar que los servicios estén corriendo
docker-compose -f docker-compose.dev.yml ps
```

Deberías ver algo como:

```
NAME                        STATUS              PORTS
respicare-mongodb-dev       Up (healthy)        0.0.0.0:27017->27017/tcp
respicare-mongo-express-dev Up                  0.0.0.0:8081->8081/tcp
respicare-ai-dev            Up (healthy)        0.0.0.0:8000->8000/tcp
```

### Paso 3: Ejecutar la Aplicación

1. Abre el proyecto en Visual Studio:
   - `Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.sln`

2. Presiona **F5** o haz clic en el botón **Start** (IIS Express)

3. La aplicación se abrirá automáticamente en: `https://localhost:44367/`

---

## 🌐 URLs del Sistema

Una vez que todo esté ejecutándose, tendrás acceso a:

| Servicio | URL | Credenciales |
|----------|-----|--------------|
| **ASP.NET MVC** | https://localhost:44367/ | - |
| **Dashboard** | https://localhost:44367/DashboardRespiCare | - |
| **MongoDB Express** | http://localhost:8081 | admin / admin123 |
| **AI Services API** | http://localhost:8000/docs | - |
| **Health Check IA** | http://localhost:8000/api/v1/health | - |

---

## 🔧 Verificación del Sistema

### 1. Verificar MongoDB

```bash
# Ver logs de MongoDB
docker logs respicare-mongodb-dev

# Conectarse a MongoDB
docker exec -it respicare-mongodb-dev mongosh -u admin -p password123
```

Dentro de mongosh:

```javascript
use respicare
show collections
db.usuarios.find().count()
```

### 2. Verificar AI Services

```bash
# Ver logs de AI Services
docker logs respicare-ai-dev

# Probar health check
curl http://localhost:8000/api/v1/health
```

O abre en tu navegador: http://localhost:8000/docs

### 3. Verificar ASP.NET MVC

Abre en tu navegador: https://localhost:44367/DashboardRespiCare/EstadoServicioIA

Deberías ver el estado de conexión con MongoDB y AI Services.

---

## 📝 Configuración

### Web.config

El archivo `Web.config` ya está configurado con los valores por defecto:

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

**Si necesitas cambiar algo**, edita estos valores en el `Web.config`.

---

## 🐛 Solución de Problemas

### Problema 1: "No se puede conectar a MongoDB"

**Solución:**

```bash
# Reiniciar MongoDB
docker restart respicare-mongodb-dev

# Ver logs para diagnosticar
docker logs respicare-mongodb-dev

# Verificar que el puerto 27017 esté libre
netstat -ano | findstr :27017
```

### Problema 2: "No se puede conectar al servicio de IA"

**Solución:**

```bash
# Reiniciar AI Services
docker restart respicare-ai-dev

# Ver logs
docker logs respicare-ai-dev

# Verificar que el puerto 8000 esté libre
netstat -ano | findstr :8000
```

### Problema 3: "Error al restaurar paquetes NuGet"

**Solución:**

```powershell
# Limpiar cache de NuGet
dotnet nuget locals all --clear

# O en la consola de NuGet Package Manager
Clear-Package -Force

# Restaurar paquetes
Update-Package -reinstall
```

### Problema 4: "Puerto 44367 ya en uso"

**Solución:**

1. Edita el archivo `.csproj` o `.csproj.user`
2. Cambia el puerto IIS Express a otro disponible (ej: 44368)
3. O cierra otras aplicaciones que usen ese puerto

### Problema 5: Docker no inicia

**Solución:**

```bash
# Verificar que Docker Desktop esté corriendo
docker --version

# Reiniciar Docker Desktop desde el menú de Windows

# Si persiste, recrear los contenedores
docker-compose -f docker-compose.dev.yml down
docker-compose -f docker-compose.dev.yml up -d --force-recreate
```

---

## 📊 Comandos Útiles

### Docker

```bash
# Ver estado de servicios
docker-compose -f docker-compose.dev.yml ps

# Ver logs en tiempo real
docker-compose -f docker-compose.dev.yml logs -f

# Ver logs de un servicio específico
docker logs respicare-ai-dev -f

# Detener todos los servicios
docker-compose -f docker-compose.dev.yml down

# Reiniciar servicios
docker-compose -f docker-compose.dev.yml restart

# Eliminar todo (incluyendo volúmenes)
docker-compose -f docker-compose.dev.yml down -v
```

### MongoDB

```bash
# Conectarse a MongoDB
docker exec -it respicare-mongodb-dev mongosh -u admin -p password123

# Backup de base de datos
docker exec respicare-mongodb-dev mongodump --out /backup --authenticationDatabase admin -u admin -p password123

# Restaurar base de datos
docker exec respicare-mongodb-dev mongorestore /backup --authenticationDatabase admin -u admin -p password123
```

### Visual Studio

```powershell
# Limpiar solución
Clean-Solution

# Reconstruir solución
Rebuild-Solution

# Restaurar paquetes NuGet
Update-Package -reinstall
```

---

## 🎯 Funcionalidades Disponibles

### Dashboard Principal
- URL: `/DashboardRespiCare/Index`
- Estadísticas generales del sistema
- Gráficos de usuarios y reportes
- Accesos rápidos

### Reportes de Síntomas
- URL: `/ReporteSintomas/Index`
- Crear nuevo reporte
- Ver reportes existentes
- Análisis automático con IA
- Vista de casos urgentes

### Historiales Médicos
- URL: `/HistorialMedicoRespiCare/Index`
- CRUD completo de historiales
- Búsqueda y filtros
- Vista por paciente/doctor

### Usuarios
- URL: `/Usuario/Index`
- Gestión de usuarios
- Roles: Patient, Doctor, Admin

### Analytics
- URL: `/DashboardRespiCare/Analytics`
- Estadísticas avanzadas
- Gráficos y tendencias

### Mapa Interactivo
- URL: `/DashboardRespiCare/MapaInteractivo`
- Visualización geográfica de casos

---

## 📋 Checklist de Inicio

Antes de comenzar a desarrollar, verifica que:

- [ ] Docker Desktop esté instalado y corriendo
- [ ] Visual Studio 2019/2022 esté instalado
- [ ] .NET Framework 4.8 esté instalado
- [ ] Paquetes NuGet restaurados correctamente
- [ ] Servicios Docker iniciados (`docker-compose ps` muestra todos como "Up")
- [ ] ASP.NET MVC ejecutándose en Visual Studio
- [ ] MongoDB Express accesible en http://localhost:8081
- [ ] AI Services accesible en http://localhost:8000/docs

---

## 📚 Recursos Adicionales

- **Documentación MongoDB C#**: https://www.mongodb.com/docs/drivers/csharp/
- **ASP.NET MVC Tutorial**: https://docs.microsoft.com/en-us/aspnet/mvc/mvc5
- **Docker Documentation**: https://docs.docker.com/
- **Bootstrap 5 Documentation**: https://getbootstrap.com/docs/5.3/
- **Chart.js Documentation**: https://www.chartjs.org/docs/

---

## 💡 Tips para Desarrollo

1. **Hot Reload**: Visual Studio recarga automáticamente al guardar cambios en vistas
2. **Debugging**: Usa puntos de interrupción (F9) para depurar código C#
3. **Logs**: Revisa la ventana "Output" en Visual Studio para ver logs
4. **MongoDB UI**: Usa Mongo Express para ver y editar datos fácilmente
5. **API Testing**: Usa la interfaz Swagger de AI Services para probar endpoints

---

## 🎉 ¡Listo para Desarrollar!

Si todo está funcionando correctamente, deberías poder:

1. ✅ Ver el Dashboard principal con estadísticas
2. ✅ Crear nuevos reportes de síntomas
3. ✅ Ver reportes con análisis de IA
4. ✅ Navegar entre las diferentes secciones
5. ✅ Acceder a MongoDB Express
6. ✅ Ver la documentación de AI Services

---

## 📞 Soporte

Si encuentras problemas:

1. Revisa la sección "Solución de Problemas" arriba
2. Verifica los logs de Docker: `docker-compose -f docker-compose.dev.yml logs`
3. Revisa los logs de Visual Studio en la ventana "Output"
4. Consulta la documentación en `README_MIGRACION.md`

---

**¡Feliz desarrollo! 🚀**

RespiCare ASP.NET MVC - Sistema Integral de Enfermedades Respiratorias 🏥

