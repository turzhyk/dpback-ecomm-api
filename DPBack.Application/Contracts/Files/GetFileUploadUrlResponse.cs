namespace DPBack.Application.Contracts.Files;

public record GetFileUploadUrlResponse(
    string UploadLink,
    IReadOnlyCollection<string> FileKeys
);