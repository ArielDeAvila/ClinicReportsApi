using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services.Interfaces;

public interface ILoginService
{
    Task<BaseResponse<string>> Login(LoginRequestDTO requestDto);
    Task<BaseResponse<string>> LoginDoctor(LoginRequestDTO requestDto);
}
