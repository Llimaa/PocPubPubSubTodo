using System.Text.Json;
using PubPubSubTodo;
using PubPubSubTodo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.MapPost("/", async (Envelope envelope, ITodoRepository todoRepository) =>
{
    if (envelope?.Message?.Data is null)
    {
        app.Logger.LogWarning("Bad Request: Invalid Pub/Sub message format.");
        return Results.BadRequest();
    }

    var data = Convert.FromBase64String(envelope.Message.Data);
    var target = System.Text.Encoding.UTF8.GetString(data);

    var todo = JsonSerializer.Deserialize<Todo>(target, JsonSerializerOptions.Default);

    await todoRepository.AddAsync(todo);

    app.Logger.LogInformation($"Todo With name: {todo?.Name} Added");

    return Results.NoContent();
});

app.Run();
