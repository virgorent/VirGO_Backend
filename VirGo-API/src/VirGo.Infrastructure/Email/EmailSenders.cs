using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Microsoft.Extensions.Logging;
using VirGo.Application.Email;

namespace VirGo.Infrastructure.Email;

/// <summary>Sends through Amazon SES v2 from the verified sender identity.</summary>
public sealed class SesEmailSender(IAmazonSimpleEmailServiceV2 ses, string senderAddress) : IEmailSender
{
    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        var request = new SendEmailRequest
        {
            FromEmailAddress = senderAddress,
            Destination = new Destination { ToAddresses = [message.To] },
            Content = new EmailContent
            {
                Simple = new Message
                {
                    Subject = new Content { Data = message.Subject, Charset = "UTF-8" },
                    Body = new Body
                    {
                        Text = new Content { Data = message.TextBody, Charset = "UTF-8" },
                        Html = new Content { Data = message.HtmlBody, Charset = "UTF-8" }
                    }
                }
            }
        };
        return ses.SendEmailAsync(request, cancellationToken);
    }
}

/// <summary>Local and test sender: logs the subject and the masked address, never the body or the code.</summary>
public sealed partial class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        cancellationToken.ThrowIfCancellationRequested();
        var maskedAddress = EmailTemplates.Mask(message.To);
        LogEmail(logger, message.Subject, maskedAddress);
        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Email '{Subject}' to {MaskedAddress} (not sent: no SES sender configured).")]
    private static partial void LogEmail(ILogger logger, string subject, string maskedAddress);
}
