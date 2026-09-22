namespace DPBack.Application.Abstractions;

public interface IDatabaseInitializer
{
    public Task InitializeDatabaseAsync();
}