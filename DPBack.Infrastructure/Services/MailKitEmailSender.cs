using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DPBack.Infrastructure.Services;

public class MailKitEmailSender : IEmailSender
{
    public async Task SendReceiptEmailAsync(string targetEmail, byte[] receiptPdf, string filename, SmtpOptions options,
        CancellationToken cToken)
    {
        if(options.Login == null || options.SmtpKey == null)
            throw new ApplicationException("Missing smtp login or smtp key. Check your configuration");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.SenderName, options.SenderEmail));
        message.To.Add(MailboxAddress.Parse("yasosbib@yahoo.com"));
        message.Subject = "Your receipt [TEST]";
        var body = new BodyBuilder { TextBody = "Thanks you for your purchase! Here is your receipt [TEST]" };
        body.Attachments.Add(filename, receiptPdf, new ContentType("application", "pdf"));
        message.Body = body.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(options.Host, options.Port, SecureSocketOptions.Auto, cToken);
        await smtp.AuthenticateAsync(options.Login, options.SmtpKey, cToken);
        await smtp.SendAsync(message, cToken);
        await smtp.DisconnectAsync(true, cToken);
    }
}