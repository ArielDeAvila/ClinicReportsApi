using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IHospitalService
{
    Task<BaseResponse<HospitalDTO>> GetById(int id);
    Task<BaseResponse<IEnumerable<HospitalDTO>>> GetAll();
    Task<BaseResponse<IEnumerable<HospitalNameDTO>>> GetAllNames();
    Task<BaseResponse<bool>> Create(HospitalRegisterDTO hospital);
    Task<BaseResponse<bool>> Update(HospitalDTO hospital);
    Task<BaseResponse<bool>> Remove(int id);
    Task<BaseResponse<bool>> UpdateEmail(UpdateEmailDTO emailDto);
    Task<BaseResponse<bool>> UpdatePassword(UpdatePasswordDTO passwordDto, int id);
}
