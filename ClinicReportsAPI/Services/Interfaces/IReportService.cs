using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.Services.Interfaces;

public interface IReportService
{
    Task<BaseResponse<ReportDTO>> GetById(int id);
    Task<BaseResponse<IEnumerable<ReportDTO>>> GetAllReportsByHospital(int hospitalId);
    Task<BaseResponse<IEnumerable<ReportDTO>>> GetAllReportsByDoctor(int doctorId);
    Task<BaseResponse<IEnumerable<ReportDTO>>> GetAllReportsByPatient(int patientId);
    Task<BaseResponse<bool>> Create(ReportDTO reportDto);
    Task<BaseResponse<bool>> Update(ReportDTO reportDto);
    Task<BaseResponse<bool>> Remove(int id);
}
