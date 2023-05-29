using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Services.Interfaces;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string emailTo, string token)
    {
        string verifyUrl = $"{_configuration["Application:BaseUrl"]}/email-validation?{token}";

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
}
