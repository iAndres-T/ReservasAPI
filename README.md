docker run --name sqlserver2022 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TG#67733200b" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

docker exec -it sqlserver2022 /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P TG#67733200b -Q "CREATE DATABASE DBReservasAPI; USE DBReservasAPI; CREATE TABLE dbo.Reserva (IdReserva INT IDENTITY(1,1) NOT NULL PRIMARY KEY, IdCliente INT NOT NULL, FechaReserva DATETIME NOT NULL, FechaInicio DATETIME NOT NULL, FechaFin DATETIME NOT NULL, Cantidad INT NULL, Estado VARCHAR(20) NOT NULL, Observaciones NVARCHAR(250) NULL, FechaCreacion DATETIME NULL); ALTER TABLE dbo.Reserva ADD DEFAULT ((1)) FOR Cantidad; ALTER TABLE dbo.Reserva ADD DEFAULT ('Pendiente') FOR Estado; ALTER TABLE dbo.Reserva ADD DEFAULT (GETDATE()) FOR FechaCreacion;"

docker network create mynetwork

docker network connect mynetwork sqlserver2022

docker network connect mynetwork ReservasAPI

docker network inspect mynetwork
