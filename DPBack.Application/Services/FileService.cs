using DPBack.Application.Abstractions;
using DPBack.Application.Contracts.Files;

namespace DPBack.Application.Services;

public class FileService : IFileService
{
    public Task<GetFileUploadUrlResponse> CreateFilesMetadataAsync(FileUploadRequest file, CancellationToken cToken)
    {
        throw new NotImplementedException();
    }
}