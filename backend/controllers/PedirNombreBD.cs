using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using CheDescifraMe.Backend.Models; // Asegúrate de usar el namespace correcto de la clase Frase
using DotNetEnv; // Asegúrate de incluir DotNetEnv para cargar las variables del .env


//En realidad no lee el nombre sino que todas las frases nomas
[ApiController]
[Route("api/[controller]")]
public class PedirNombreBDController : ControllerBase
{
    private readonly string _connectionString;

    // Constructor que recibe la cadena de conexión de la base de datos
    public PedirNombreBDController()
    {
        // Cargar las variables desde el archivo .env
        Env.Load();  // Esto carga las variables del archivo .env

        // Leer las variables de entorno
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "usuario";
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "contraseña";
        var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "mi_base_de_datos";

        // Construir la cadena de conexión con las variables obtenidas
        _connectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={dbName}";
    }

    [HttpGet]
    public IActionResult GetFrases()
    {
        // Lista para almacenar las frases obtenidas de la base de datos
        List<Frase> frases = new List<Frase>();

        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Consulta SQL para obtener todas las frases
                var query = "SELECT * FROM frases";
                using (var cmd = new NpgsqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    // Leer los datos
                    while (reader.Read())
                    {
                        frases.Add(new Frase
                        {
                            Id = reader.GetInt32(0),    // Primer columna: Id
                            Contenido = reader.GetString(1)  // Segunda columna: Contenido
                        });
                    }
                }
            }

            // Devolver las frases como respuesta
            return Ok(frases);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al obtener las frases: {ex.Message}" });
        }
    }
}
