using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public override async void Update(Patient patient)
    {
        var existingPatient = await GetById(patient.Id);

        existingPatient.AuditDateUpdate = DateTime.Now;
        
    }

    //TODO: crear nuevos métodos para el cambio de contraseña y de email

}
