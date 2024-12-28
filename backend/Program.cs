using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Ruta al archivo .env en la carpeta anterior
string envFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, ".env");
// Intentamos cargar las variables de entorno desde la ruta especificada
Env.Load(envFilePath);

// Leer variables de entorno desde el archivo .env
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "usuario";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "contraseña";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "mi_base_de_datos";

// Crear la cadena de conexión
var connectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={dbName}";

// Crear y abrir una conexión a la base de datos
using var connection = new NpgsqlConnection(connectionString);
connection.Open();

// Configurar CORS para permitir el acceso desde el frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var frontendHost = Environment.GetEnvironmentVariable("VITE_BACKEND_HOST") ?? "http://localhost";
        var frontendPort = Environment.GetEnvironmentVariable("VITE_BACKEND_PORT") ?? "5136";

        var frontendUrl = $"{frontendHost}:{frontendPort}";

        policy.AllowAnyOrigin()
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
