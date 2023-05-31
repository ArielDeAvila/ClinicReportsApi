using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(SystemReportContext context) : base(context)
    {
        
    }

    public override async Task<IEnumerable<Patient>> GetAll()
    {
        var patients = await _context.Patients.AsNoTracking().Where(p => p.AuditDateDelete==null)
                                        .Include(p => p.Hospital).ToListAsync();

        return patients;
    }

    public override async Task<Patient> GetById(int id)
    {
        var patient = await _context.Patients.AsNoTracking()
                                             .Include(p => p.Hospital)
                                             .FirstOrDefaultAsync(p => p.Id.Equals(id) && p.AuditDateDelete == null);

        return patient!;
    }

    public async Task<Patient> GetPatient(Expression<Func<Patient, bool>> expression)
    {
        var patient = await _context.Patients.AsNoTracking()
                                             .Include(p => p.Hospital)
                                             .FirstOrDefaultAsync(expression);

        return patient;
    }  

    public override async void Update(Patient patient)
    {
        var existingPatient = await GetById(patient.Id);

        existingPatient.AuditDateUpdate = DateTime.Now;
        
    }

    public async Task<Patient> GetByIdentification(string identification)
    {
        var patient = await _context.Patients.AsNoTracking()
                                             .Include(p => p.Hospital)
                                             .FirstOrDefaultAsync(p => p.Identification.Equals(identification) && p.AuditDateDelete == null);

        return patient!;
    }



    public void UpdateEmail(Patient patient)
    {
        _context.Patients.Update(patient);
        _context.Entry(patient).Property(d => d.Identification).IsModified = false;
        _context.Entry(patient).Property(d => d.Password).IsModified = false;

    }

    public void UpdatePassword(Patient patient)
    {
        _context.Patients.Update(patient);
        _context.Entry(patient).Property(d => d.Identification).IsModified = false;
        _context.Entry(patient).Property(d => d.Email).IsModified = false;
    }

}
