using System.Globalization;
using System.Net;

namespace VirGo.Application.Email;

public sealed record EmailMessage(string To, string Subject, string TextBody, string HtmlBody);

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}

public static class EmailTemplates
{
    public const string PasswordReset = "password-reset";
    public const string EmailVerification = "email-verification";

    public static bool IsKnown(string template) =>
        template is PasswordReset or EmailVerification;

    public static EmailMessage Build(string template, string to, string code, int expiresInMinutes)
    {
        var (subject, intro, ignore) = template switch
        {
            PasswordReset => (
                "Tu código para recuperar tu contraseña de VirGo",
                "Recibimos una solicitud para cambiar la contraseña de tu cuenta de VirGo. Usa este código:",
                "Si no la pediste, ignora este correo: tu contraseña no cambiará."),
            EmailVerification => (
                "Verifica tu correo en VirGo",
                "Gracias por registrarte en VirGo. Para confirmar tu correo, usa este código:",
                "Si no creaste una cuenta en VirGo, ignora este correo."),
            _ => throw new ArgumentOutOfRangeException(nameof(template), template, "Unknown email template.")
        };

        var minutes = expiresInMinutes.ToString(CultureInfo.InvariantCulture);
        var text = $"{intro}\n\n{code}\n\nEl código vence en {minutes} minutos.\n\n{ignore}\n\nEquipo VirGo";
        var html =
            $"<p>{WebUtility.HtmlEncode(intro)}</p>" +
            $"<p style=\"font-size:24px;font-weight:bold;letter-spacing:4px\">{WebUtility.HtmlEncode(code)}</p>" +
            $"<p>El código vence en {minutes} minutos.</p>" +
            $"<p>{WebUtility.HtmlEncode(ignore)}</p><p>Equipo VirGo</p>";
        return new EmailMessage(to, subject, text, html);
    }

    /// <summary>Masks an address for logs: ana.perez@gmail.com → a***@gmail.com.</summary>
    public static string Mask(string address)
    {
        var at = address.IndexOf('@', StringComparison.Ordinal);
        return at <= 0 ? "***" : $"{address[0]}***{address[at..]}";
    }
}
