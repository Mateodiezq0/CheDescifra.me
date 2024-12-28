using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using frontend;
using DotNetEnv;  // Importar DotNetEnv para leer el .env

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Cargar las variables del archivo .env
Env.Load();  // Cargar las variables de entorno del archivo .env

// Obtener la URL del backend desde las variables de entorno
var backendHost = Environment.GetEnvironmentVariable("VITE_BACKEND_HOST") ?? "http://localhost";
var backendPort = Environment.GetEnvironmentVariable("VITE_BACKEND_PORT") ?? "5136";
var backendUrl = $"{backendHost}:{backendPort}";

// Mostrar la URL completa en la consola (para depuración)
Console.WriteLine($"Backend URL: {backendUrl}");

// Configurar el HttpClient con la URL del backend
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendUrl) });

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
