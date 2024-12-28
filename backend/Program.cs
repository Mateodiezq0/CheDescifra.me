using DotNetEnv; // Si estás usando DotNetEnv para cargar variables del .env
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables del archivo .env
Env.Load();

// Leer variables de entorno desde el archivo .env
var backendHost = Environment.GetEnvironmentVariable("BACKEND_HOST") ?? "localhost";
var backendPort = Environment.GetEnvironmentVariable("BACKEND_PORT") ?? "5000"; // Valor por defecto

// Construir la URL del backend
var backendUrl = $"http://{backendHost}:{backendPort}";

// Configurar CORS para permitir el acceso desde el frontend (ajusta el puerto si es necesario)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        // Usar la URL del frontend desde el archivo .env
        var frontendHost = Environment.GetEnvironmentVariable("VITE_BACKEND_HOST") ?? "http://localhost";
        var frontendPort = Environment.GetEnvironmentVariable("VITE_BACKEND_PORT") ?? "5136"; // Valor por defecto

        var frontendUrl = $"{frontendHost}:{frontendPort}";

        // Configurar CORS con la URL del frontend
        policy.AllowAnyOrigin() //ACÁ SE PUEDE HACER PETICION DESDE CUALQUIER LADO, CAMBIAR DESPUES?????
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Agregar servicios al contenedor de dependencias
builder.Services.AddControllers();

var app = builder.Build();

// Usar CORS
app.UseCors();

// Configurar los endpoints
app.MapControllers();

app.Run();
