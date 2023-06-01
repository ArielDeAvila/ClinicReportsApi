using ClinicReportsAPI.Data.Entities;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories.Interfaces;

public interface IReportRepository : IGenericRepository<Report>
{
    Task<IEnumerable<Report>> GetAllReportsByHospital(int hospitalId);
    Task<IEnumerable<Report>> GetAllReportsByDoctor(int doctorId);
    Task<IEnumerable<Report>> GetAllReportsByPatient(int patientId);
    Task<Report> GetReport(Expression<Func<Report, bool>> expression);
    
}
