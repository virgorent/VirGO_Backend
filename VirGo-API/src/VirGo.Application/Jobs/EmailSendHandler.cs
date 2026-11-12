using System.Globalization;
using Microsoft.Extensions.Logging;
using VirGo.Application.Email;

namespace VirGo.Application.Jobs;

/// <summary>Sends a templated email. Never logs the code or the full address.</summary>
public sealed partial class EmailSendHandler(IEmailSender sender, ILogger<EmailSendHandler> logger) : IJobHandler
{
    private const int DefaultExpiresInMinutes = 15;

    public string Type => JobTypes.EmailSend;

    public async Task HandleAsync(JobMessage message, CancellationToken cancellationToken = default)
    {
        var template = message.Get("template");
        if (!EmailTemplates.IsKnown(template))
        {
            throw new InvalidJobMessageException($"Unknown email template '{template}'.");
        }

        var to = message.Get("to");
        var expires = message.Data is not null
            && message.Data.TryGetValue("expiresInMinutes", out var raw)
            && int.TryParse(raw, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed)
            && parsed is > 0 and <= 1440
                ? parsed
                : DefaultExpiresInMinutes;

        await sender.SendAsync(EmailTemplates.Build(template, to, message.Get("code"), expires), cancellationToken);
        var maskedAddress = EmailTemplates.Mask(to);
        LogSent(logger, template, maskedAddress);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Email {Template} sent to {MaskedAddress}.")]
    private static partial void LogSent(ILogger logger, string template, string maskedAddress);
}
