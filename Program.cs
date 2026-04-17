using Microsoft.Data.SqlClient;
var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllers();

// Учимся читать конфигурацию
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("App"));

var app = builder.Build();

// Настройка HTTP pipeline
app.UseHttpsRedirection();
app.MapControllers();

// Эндпоинт /health
app.MapGet("/health", () => new
{
    status = "ok",
    timestamp = DateTime.UtcNow
});

// Эндпоинт /version
app.MapGet("/version", (IConfiguration config) => new
{
    name = config["App:Name"] ?? "IsLabApp",
    version = config["App:Version"] ?? "0.1.0"
});

app.Run();
// Эндпоинт /db/ping
app.MapGet("/db/ping", async (IConfiguration config) =>
{
    var connectionString = config.GetConnectionString("Mssql");
    try
    {
        await using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        await connection.OpenAsync();
        return Results.Ok(new { status = "ok", message = "Database connection successful" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { status = "error", message = ex.Message });
    }
});
// Класс для настроек
public class AppSettings
{
    public string Name { get; set; } = "";
    public string Version { get; set; } = "";
}
