using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IDoctorService
{
    Task<BaseResponse<IEnumerable<DoctorDTO>>> GetAll();
    Task<BaseResponse<IEnumerable<DoctorNameDTO>>> GetAllNames();
    Task<BaseResponse<DoctorDTO>> GetById(int id);
    Task<BaseResponse<bool>> Create(DoctorRegisterDTO dto);
    Task<BaseResponse<bool>> Update(DoctorDTO dto);
    Task<BaseResponse<bool>> Remove(int id);

}
