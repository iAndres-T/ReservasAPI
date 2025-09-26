# PRACTICA #2 - Despliegue de una API REST en contenedores Docker y Kubernetes

## 👥 Integrantes
- Andres Tabares Cardona  
- Efrén Felipe Cuadrado Barboza  
- Franco Moncayo Uribe  
- Juan Pablo Gomez  
- Evelyn Muñetones Alvarez  

📄 **Enlace documento/video de evidencias:**  
[Ver documento en Google Docs](https://docs.google.com/document/d/11X9rrTQG4-m5Giw_T1g4-T1xDU09JI8spsTSU4ToNqc/edit?usp=sharing)
[Ver Video Evidencia Drive](https://drive.google.com/file/d/17332MDHoZT6RVzNQSDi_MRzUc0EUVfcR/view?usp=sharing)

---

## 🚀 Despliegue con Docker  

1. **Ejecutar contenedor de SQL Server 2022:**  
```bash
docker run --name sqlserver2022 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TG#67733200b" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

2. **Crear la base de datos:**  
```bash
docker exec -it sqlserver2022 /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P 'TG#67733200b' -Q "CREATE DATABASE DBReservasAPI;"
```

3. **Crear la tabla Reserva:**  
```bash
docker exec -it sqlserver2022 /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P 'TG#67733200b' -d DBReservasAPI -Q "CREATE TABLE dbo.Reserva (IdReserva INT IDENTITY(1,1) NOT NULL PRIMARY KEY, IdCliente INT NOT NULL, FechaReserva DATETIME NOT NULL, FechaInicio DATETIME NOT NULL, FechaFin DATETIME NOT NULL, Cantidad INT NULL, Estado VARCHAR(20) NOT NULL, Observaciones NVARCHAR(250) NULL, FechaCreacion DATETIME NULL); ALTER TABLE dbo.Reserva ADD DEFAULT ((1)) FOR Cantidad; ALTER TABLE dbo.Reserva ADD DEFAULT ('Pendiente') FOR Estado; ALTER TABLE dbo.Reserva ADD DEFAULT (GETDATE()) FOR FechaCreacion;"
```

4. **Crear red para comunicación entre contenedores:**  
```bash
docker network create mynetwork
docker network connect mynetwork sqlserver2022
docker network connect mynetwork ReservasAPI
```

5. **Validar que ambos contenedores estén en la red:**  
```bash
docker network inspect mynetwork
```

👉 Luego ejecutar el contenedor de **ReservasAPI** desde **Visual Studio** y validar en **Swagger**.  

---

## ☸️ Despliegue con Kubernetes  

### Requisitos previos  
- Tener habilitado **Kubernetes en Docker Desktop**.  
- Realizar los siguientes cambios en el código de `ReservasAPI`:  
  1. En `appsettings.json`, dejar `DefaultConnection` vacío (`""`).  
  2. En `Program.cs`, descomentar:  
     ```csharp
     builder.WebHost.UseUrls("http://*:80");
     ```

---

### Pasos de despliegue  

1. **Generar imagen de la API (desde el proyecto):**  
```bash
docker build -t reservas-api-kub:local .
```

2. **Aplicar los deployments:**  
```bash
kubectl apply -f deployment.yaml
kubectl apply -f deploymentsqlserver.yaml
```

3. **Verificar pods creados y corriendo:**  
```bash
kubectl get pods
```

4. **Verificar servicios:**  
```bash
kubectl get svc
```

---

### Creación de la base de datos en SQL Server dentro de Kubernetes  

1. **Entrar al pod de SQL Server** (cambiar `nombre-pod-sqlserver` por el nombre real del pod obtenido antes):  
```bash
kubectl exec -it nombre-pod-sqlserver -- /bin/bash
```

👉 El prompt debería verse como:  
```
mssql@sqlserver-deployment-xxxxx:/#
```

2. **Crear la base de datos:**  
```bash
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P 'TG#67733200b' -Q "CREATE DATABASE DBReservasAPI;"
```

3. **Crear la tabla Reserva:**  
```bash
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P 'TG#67733200b' -d DBReservasAPI -Q "CREATE TABLE dbo.Reserva (IdReserva INT IDENTITY(1,1) NOT NULL PRIMARY KEY, IdCliente INT NOT NULL, FechaReserva DATETIME NOT NULL, FechaInicio DATETIME NOT NULL, FechaFin DATETIME NOT NULL, Cantidad INT NULL, Estado VARCHAR(20) NOT NULL, Observaciones NVARCHAR(250) NULL, FechaCreacion DATETIME NULL); ALTER TABLE dbo.Reserva ADD DEFAULT ((1)) FOR Cantidad; ALTER TABLE dbo.Reserva ADD DEFAULT ('Pendiente') FOR Estado; ALTER TABLE dbo.Reserva ADD DEFAULT (GETDATE()) FOR FechaCreacion;"
```

---

## ✅ Validación final  
Abrir en el navegador:  
```
http://localhost:30080/swagger/index.html
```
