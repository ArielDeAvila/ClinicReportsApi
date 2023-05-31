using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace ClinicReportsAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string emailTo, string token, string rol)
    {
        string verifyUrl = $"{_configuration["Application:BaseUrl"]}/{rol}/email-validation?{token}";

        //Configuración del correo
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Email:User"]));
        email.To.Add(MailboxAddress.Parse(emailTo));
        email.Subject = "Valida tu dirección de correo electrónico";
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = EmailMessage.BodyMessage(verifyUrl)
        };

        //configuración del servidor
        using var smtp = new SmtpClient();
        smtp.Connect(
                _configuration["Email:Host"],
                int.Parse(_configuration["Email:Port"]!),
                SecureSocketOptions.StartTls
            );

        smtp.Authenticate(_configuration["Email:User"], _configuration["Email:Password"]);

        smtp.Send(email);

        smtp.Disconnect(true);
    }

    public void SendCredentials(BaseUser account)
    {
        string applicationUrl = _configuration["Application:BaseUrl"]!;

        //configuración del correo
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Email:User"]));
        email.To.Add(MailboxAddress.Parse(account.Email));
        email.Subject = "Registrado en el sistema";
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = EmailMessage.CredentialMessage(account, applicationUrl)
        };

        //configuración del servidor
        using var smtp = new SmtpClient();
        smtp.Connect(
                _configuration["Email:Host"],
                int.Parse(_configuration["Email:Port"]!),
                SecureSocketOptions.StartTls
            );
        smtp.Authenticate(_configuration["Email:User"], _configuration["Email:Password"]);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
