using DPBack.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DPBack.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;

    public LocalFileStorage(IConfiguration configuration)
    {
        _rootPath = configuration["FileStorage:RootPath"] ?? throw new InvalidOperationException("no file path found");
    }

    public async Task SaveAsync(string key, Stream content, CancellationToken cToken)
    {
        var fullPath = Path.Combine(_rootPath, key);
        var directory = Path.GetDirectoryName(fullPath);

        if (directory is not null)
            Directory.CreateDirectory(directory);
        await using var fileStream = new FileStream(
            fullPath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 64 * 1024,
            useAsync: true);

        await content.CopyToAsync(
            fileStream,
            cToken);
    }

    public Task<Stream> Read(string key, CancellationToken cToken)
    {
        var fullPath = Path.Combine(_rootPath, key);
        Stream stream = new FileStream(
            fullPath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 64 * 1024,
            useAsync: true);
        return Task.FromResult(stream);
    }

    public Task Delete(string key, CancellationToken cToken)
    {
        var fullPath = Path.Combine(_rootPath, key);
        if(File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }
}