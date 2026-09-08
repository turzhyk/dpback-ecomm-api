using DPBack.Application.Options;

namespace DPBack.Application.Abstractions;

public interface IEmailSender
{
    Task SendReceiptEmailAsync(string targetEmail, SmtpOptions options, byte[] receiptPdf, string filename, CancellationToken cToken);
}