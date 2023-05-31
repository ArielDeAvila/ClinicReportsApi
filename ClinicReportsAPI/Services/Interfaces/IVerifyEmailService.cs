using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IVerifyEmailService
{
    Task<BaseResponse<bool>> VerifyEmail(VerifyEmailDTO dto);
}
