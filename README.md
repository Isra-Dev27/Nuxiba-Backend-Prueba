# Nuxiba Backend Prueba

**Autor:** Israel Velarde

Repositorio con la solución a la prueba práctica en .NET 8 y SQL Server.

## 🚀 Requisitos Previos

Tener instalado Docker en el equipo. (La solución está completamente dockerizada para evitar dependencias locales).

---

## 🛠️ 1. Levantar el contenedor de SQL Server

Para levantar el contenedor de la base de datos con las configuraciones requeridas por la prueba, abre tu terminal y ejecuta:

```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd' -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
```

## 🔌 2. Conectar la base de datos

Puedes conectarte usando SQL Server Management Studio (SSMS) o Azure Data Studio con estas credenciales:
- **Servidor:** localhost, puerto 1433
- **Usuario:** sa
- **Contraseña:** YourStrong!Passw0rd

*(Nota: La API creará automáticamente la base de datos `NuxibaDB` y las tablas necesarias al arrancar por primera vez).*

---

## 🐳 3. Ejecutar la API y sus endpoints

Abre una nueva terminal en la raíz de este proyecto (`NUXIBA - Backend`) y ejecuta:

```bash
docker compose up --build
```
Esto construirá y levantará la API de .NET. Puedes probar los endpoints entrando a la interfaz de Swagger en tu navegador:
👉 [http://localhost:8080/swagger](http://localhost:8080/swagger)

**Endpoints principales:**
- **GET /api/logins**: Obtiene todos los registros.
- **POST /api/logins**: Registra un nuevo login/logout (validando reglas de negocio).
- **PUT y DELETE**: Actualización y eliminación estándar.

---

## 📄 4. Descargar el CSV generado

Para descargar el archivo `.csv` con el reporte de horas trabajadas (Ejercicio 3):

1. En la página de Swagger, ve al endpoint **GET `/api/Reports/logins-csv`** y ejecútalo.
2. En la respuesta aparecerá un enlace **"Download file"**.
3. **Alternativamente por consola:**
   ```bash
   curl -o reporte_logins.csv http://localhost:8080/api/reports/logins-csv
   ```

---

## 🔍 Ejercicio 2: Consultas SQL

Se incluye el archivo `Queries.sql` en la raíz del proyecto con el script completo para resolver las tres consultas (usuario con más tiempo logueado, menos tiempo, y promedios por mes).
