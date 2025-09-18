# EnvioPedidosAcme

API REST en **.NET 8** que recibe un JSON con información de un pedido, lo transforma en un **mensaje SOAP** conforme al WSDL de EnvioPedidosAcme, lo envía al servicio configurado (ejemplo: Beeceptor) y devuelve la respuesta SOAP en formato **JSON**.

---

## 📂 Estructura del proyecto
EnvioPedidosAcme.sln
src/
EnvioPedidosAcme.Api/ # Capa de presentación (Web API, Swagger, Program.cs)
EnvioPedidosAcme.Application/ # Abstracciones (interfaces) y Contratos (DTOs)
EnvioPedidosAcme.Domain/ # Entidades del dominio (placeholder por ahora)
EnvioPedidosAcme.Infrastructure/ # Implementaciones externas (cliente SOAP, DI)

## ⚙️ Configuración

La API usa `appsettings.json` o variables de entorno para configurar el endpoint SOAP:

```json
{
  "SoapEnvioPedidos": {
    "Endpoint": "https://smb2b095807450.free.beeceptor.com/EnvioPedidosAcmeService",
    "Namespace": "http://WSDLs/EnvioPedidos/EnvioPedidosAcme"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

---
## Restaurar dependencias
dotnet restore EnvioPedidosAcme.sln

## Compilar solución
dotnet build EnvioPedidosAcme.sln -c Release

## Ejecutar la API
dotnet run --project src/EnvioPedidosAcme.Api

Una vez levantada, abre Swagger en:
👉 http://localhost:5080/swagger
 (o el puerto que indique tu consola).

##🐳 Ejecutar con Docker
1. Construir la imagen

En la raíz del proyecto (donde está EnvioPedidosAcme.sln y Dockerfile):

docker build -t enviopedidosacme:latest .
