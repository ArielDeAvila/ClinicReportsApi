using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IEmailService
{
    void SendEmail(string emailTo, string token, string rol);
    void SendCredentials(BaseUser account);
}
