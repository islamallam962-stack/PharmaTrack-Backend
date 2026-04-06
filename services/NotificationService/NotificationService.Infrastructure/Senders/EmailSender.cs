using Microsoft.Extensions.Configuration;
using NotificationService.Application.Common.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService.Infrastructure.Senders;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config) => _config = config;

    public async Task SendAsync(
        string toEmail, string subject, string body, CancellationToken ct)
    {
        var client = new SendGridClient(_config["SendGrid:ApiKey"]!);
        var from   = new EmailAddress(_config["SendGrid:FromEmail"]!, "PharmaTrack");
        var to     = new EmailAddress(toEmail);
        var msg    = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(msg, ct);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"SendGrid failed: {response.StatusCode}");
    }
}