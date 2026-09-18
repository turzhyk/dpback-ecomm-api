namespace DPBack.Application.Options;

public class SmtpOptions
{
    public required string SenderEmail { get; init; }
    public required string SenderName { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required bool UseSsl { get; init; }
}