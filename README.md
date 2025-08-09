# BookRadar - Prueba Técnica Fullstack Semi Senior

## Requisitos
- .NET 8 SDK
- SQL Server (LocalDB, SQL Express o Docker)
- (Opcional) Docker

## Configuración rápida (Windows / Linux)
1. Clonar repo:
   git clone <tu-repo-url>
2. Configurar la cadena de conexión en `appsettings.json` (DefaultConnection).
   - Ejemplo local con Docker:
     - Ejecutar SQL Server:
       docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2022-latest
3. Crear BD:
   - Opción A (EF Core migrations):
     - dotnet tool install --global dotnet-ef
     - dotnet ef migrations add Initial
     - dotnet ef database update
   - Opción B (ejecutar scripts SQL en `sql/create_table_historial.sql` y `sql/sp_insert_historial.sql`)
4. Ejecutar la app:
   - En la carpeta `BookRadar`:
     dotnet run
   - Abrir http://localhost:5000 o la URL que muestre la consola.
5. Uso:
   - En la página principal, ingresa el nombre del autor y presiona "Buscar".
   - Los resultados se muestran y se guardan en la tabla `HistorialBusquedas`.

## Decisiones de diseño
- ASP.NET Core MVC (.NET 8): pedido en la prueba y apto para un CRUD sencillo.
- EF Core: productivo y estándar en .NET para acceso a datos; permite migraciones y testing.
- Guardado por cada libro devuelto: esto facilita análisis de qué libros fueron consultados.
- Prevención de duplicados (1 minuto): se implementa server-side (antes de insertar) y opcionalmente en un stored proc.
- UI: Bootstrap para rapidez y responsividad.

## Mejoras pendientes
- Paginación en resultados y en historial.
- Caching de respuestas de OpenLibrary para no abusar la API.
- Tests unitarios e integración (mocks para HttpClient y base de datos en memoria).
- Validaciones más robustas y manejo de errores/UI para estados de API fallida.
- Rate limiting y circuit breaker para llamadas a OpenLibrary.
