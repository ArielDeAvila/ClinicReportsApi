using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicReportsAPI.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(SystemReportContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Report>> GetAllReportsByDoctor(int doctorId)
    {
        var reports = await _context.Reports.Where(r => r.DoctorId.Equals(doctorId) && r.AuditDateDelete == null)
                                            .Include(r => r.Doctor)
                                            .Include(r => r.Hospital)
                                            .Include(r => r.Patient)
                                            .AsNoTracking().ToListAsync();

        return reports;
    }

    public async Task<IEnumerable<Report>> GetAllReportsByHospital(int hospitalId)
    {
        var reports = await _context.Reports.Where(r => r.HospitalId.Equals(hospitalId) && r.AuditDateDelete == null)
                                            .Include(r => r.Doctor)
                                            .Include(r => r.Hospital)
                                            .Include(r => r.Patient)
                                            .AsNoTracking().ToListAsync();

        return reports;
    }

    public async Task<IEnumerable<Report>> GetAllReportsByPatient(int patientId)
    {
        var reports = await _context.Reports.Where(r => r.PatientId.Equals(patientId) && r.AuditDateDelete == null)
                                            .Include(r => r.Doctor)
                                            .Include(r => r.Hospital)
                                            .Include(r => r.Patient)
                                            .AsNoTracking().ToListAsync();

        return reports;
    }

    public override async Task<Report> GetById(int id)
    {
        var report = await _context.Reports.Include(r => r.Hospital)
                                           .Include(r => r.Patient)    
                                           .Include(r => r.Doctor)
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(r => r.Id.Equals(id) && r.AuditDateDelete==null);

        return report!;

    }
}
