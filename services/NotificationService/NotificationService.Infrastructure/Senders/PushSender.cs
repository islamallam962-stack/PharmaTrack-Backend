using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using NotificationService.Application.Common.Interfaces;

namespace NotificationService.Infrastructure.Senders;

public class PushSender : IPushSender
{
    private readonly string _credentialPath;

    public PushSender(IConfiguration config)
    {
        _credentialPath = config["Firebase:CredentialPath"] ?? "";

        if (FirebaseApp.DefaultInstance != null) return;

        // لو مفيش credential file، مش هيـcrash — بس مش هيبعت
        if (!File.Exists(_credentialPath)) return;

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(_credentialPath)
        });
    }

    public async Task SendAsync(
        string deviceToken, string title, string body, CancellationToken ct)
    {
        if (FirebaseApp.DefaultInstance == null)
            throw new Exception("Firebase not initialized — check credential file path.");

        var message = new Message
        {
            Token        = deviceToken,
            Notification = new Notification { Title = title, Body = body },
            Android      = new AndroidConfig { Priority = Priority.High },
            Apns         = new ApnsConfig { Aps = new Aps { Sound = "default" } }
        };

        await FirebaseMessaging.DefaultInstance.SendAsync(message, ct);
    }
}