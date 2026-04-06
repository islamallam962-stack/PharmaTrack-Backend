using Microsoft.Extensions.Configuration;
using NotificationService.Application.Common.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService.Infrastructure.Senders;

public class SmsSender : ISmsSender
{
    private readonly IConfiguration _config;

    public SmsSender(IConfiguration config) => _config = config;

    public Task SendAsync(string toPhone, string message, CancellationToken ct)
    {
        TwilioClient.Init(
            _config["Twilio:AccountSid"]!,
            _config["Twilio:AuthToken"]!);

        MessageResource.Create(
            to:   new Twilio.Types.PhoneNumber(toPhone),
            from: new Twilio.Types.PhoneNumber(_config["Twilio:FromPhone"]!),
            body: message);

        return Task.CompletedTask;
    }
}