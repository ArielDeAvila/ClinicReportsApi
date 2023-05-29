using ClinicReportsAPI.DTOs;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IEmailService
{
    void SendEmail(string emailTo, string token);
}
