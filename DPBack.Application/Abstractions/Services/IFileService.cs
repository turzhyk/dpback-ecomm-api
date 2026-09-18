using DPBack.Application.Contracts.Files;

namespace DPBack.Application.Abstractions;

public interface IFileService
{
    Task<GetFileUploadUrlResponse> CreateFilesMetadataAsync(FileUploadRequest file, CancellationToken cToken);

}