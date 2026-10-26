using DPBack.Application.Options;

namespace DPBack.Application.Abstractions;

public interface IEmailSender
{
    Task SendReceiptEmailAsync(string targetEmail, byte[] receiptPdf, string filename, SmtpOptions options, CancellationToken cToken);
}