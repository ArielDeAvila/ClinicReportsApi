using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicReportsAPI.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(SystemReportContext context) : base(context)
    {

    }

    public override async Task<IEnumerable<Doctor>> GetAll()
    {
        var doctors = await _context.Doctors.Where(d => d.AuditDateDelete == null).AsNoTracking()
                                            .Include(d => d.Hospital).ToListAsync();   
        
        return doctors;
    }

    public override async Task<Doctor> GetById(int id)
    {
        var doctor = await _context.Doctors.Include(d => d.Hospital).AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id.Equals(id) && d.AuditDateDelete==null);

        return doctor!;
    }

    public override async void Update(Doctor doctor)
    {
        var existingDoctor = await _context.Doctors.FirstAsync(d =>d.Id.Equals(doctor.Id));

        existingDoctor.AuditDateUpdate = DateTime.Now;
        existingDoctor.Birthdate = doctor.Birthdate;
        existingDoctor.Address = doctor.Address;   
        existingDoctor.MedicalSpecialty = doctor.MedicalSpecialty;
        existingDoctor.Name = doctor.Name;
        existingDoctor.PhoneNumber = doctor.PhoneNumber;

        _context.Doctors.Update(existingDoctor);
        _context.Entry(existingDoctor).Property(d => d.Identification).IsModified = false;
    }

    //TODO: crear nuevos métodos para el cambio de contraseña y de email
}
