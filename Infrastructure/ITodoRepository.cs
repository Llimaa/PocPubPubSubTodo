namespace PubPubSubTodo.Infrastructure;

public interface ITodoRepository 
{
    public Task AddAsync(Todo todo);
}
