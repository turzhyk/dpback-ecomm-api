using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using MailKit.Net.Smtp;
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
        var body = new BodyBuilder { TextBody = "Thanks you for your purchase! Here is your receipt" };
        body.Attachments.Add(filename, receiptPdf, new ContentType("application", "pdf"));
        message.Body = body.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(options.Host, options.Port, options.UseSsl, cToken);
        await smtp.SendAsync(message, cToken);
        await smtp.DisconnectAsync(true, cToken);
    }
}