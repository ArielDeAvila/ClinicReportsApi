using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IPatientService
{
    Task<BaseResponse<IEnumerable<PatientDTO>>> GetAll();
    Task<BaseResponse<PatientDTO>> GetByIdentification(string id);
    Task<BaseResponse<bool>> Create(PatientRegisterDTO patientDTO);
    Task<BaseResponse<bool>> Update(PatientDTO patientDTO);
    Task<BaseResponse<bool>> Remove(int id);
    Task<BaseResponse<bool>> UpdateEmail(UpdateEmailDTO emailDto);
    Task<BaseResponse<bool>> UpdatePassword(UpdatePasswordDTO passwordDto, int id);
}
