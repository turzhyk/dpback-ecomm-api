using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using MimeKit;

namespace DPBack.Infrastructure.Services;

public class MailKitEmailSender:IEmailSender
{
    public async Task SendReceiptEmailAsync(string targetEmail, SmtpOptions options, byte[] receiptPdf, string filename, CancellationToken cToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.SenderName, options.SenderEmail));
        message.To.Add(MailboxAddress.Parse(targetEmail));
        message.Subject = "Your receipt";
        //..
    }
}