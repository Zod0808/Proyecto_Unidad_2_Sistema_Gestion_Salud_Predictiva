# Dockerfile para ASP.NET MVC Framework 4.8
# Imagen base de Windows Server Core con .NET Framework 4.8
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019

# Establecer el directorio de trabajo
WORKDIR /inetpub/wwwroot

# Copiar los archivos publicados de la aplicación
COPY ./bin/Release/Publish/ .

# Exponer el puerto 80
EXPOSE 80

# El contenedor usará IIS que ya viene configurado en la imagen base

