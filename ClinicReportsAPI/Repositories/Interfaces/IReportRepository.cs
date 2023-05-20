using ClinicReportsAPI.Data.Entities;

namespace ClinicReportsAPI.Repositories.Interfaces;

public interface IReportRepository : IGenericRepository<Report>
{
    Task<IEnumerable<Report>> GetAllReportsByHospital(int hospitalId);
    Task<IEnumerable<Report>> GetAllReportsByDoctor(int doctorId);
    Task<IEnumerable<Report>> GetAllReportsByPatient(int patientId);
}
