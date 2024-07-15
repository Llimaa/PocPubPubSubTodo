using MongoDB.Driver;

namespace PubPubSubTodo.Infrastructure;

public class TodoRepository: ITodoRepository
{
    private readonly IMongoCollection<Todo> collection;

    public TodoRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("connectionString");
        var mongoClient = new MongoClient(connectionString);

        var database = mongoClient.GetDatabase("Todo");
        collection = database.GetCollection<Todo>("Todo");
    }

    public async Task AddAsync(Todo todo)
    {
        await collection.InsertOneAsync(todo);
    }
}
