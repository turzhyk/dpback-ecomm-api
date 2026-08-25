namespace DPBack.Application.Abstractions;

public interface IFileStorage
{
    Task SaveAsync(string key, Stream content, CancellationToken cToken);
    Task<Stream> Read(string key, CancellationToken cToken);
    Task Delete(string key, CancellationToken cToken);
}