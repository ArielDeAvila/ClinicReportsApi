using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<Doctor> GetDoctor(Expression<Func<Doctor,bool>> expression)
    {
        var doctor = await _context.Doctors.Include(d => d.Hospital).AsNoTracking()
            .FirstOrDefaultAsync(expression);

        return doctor!;
    }

    public override async void Update(Doctor doctor)
    {
        var existingDoctor = await _context.Doctors.FirstAsync(d => d.Id.Equals(doctor.Id));

        existingDoctor.AuditDateUpdate = DateTime.Now;
        existingDoctor.Birthdate = doctor.Birthdate;
        existingDoctor.Address = doctor.Address;   
        existingDoctor.MedicalSpecialty = doctor.MedicalSpecialty;
        existingDoctor.Name = doctor.Name;
        existingDoctor.PhoneNumber = doctor.PhoneNumber;

        _context.Doctors.Update(existingDoctor);
        _context.Entry(existingDoctor).Property(d => d.Identification).IsModified = false;
        _context.Entry(existingDoctor).Property(d => d.Email).IsModified = false;
        _context.Entry(existingDoctor).Property(d => d.Password).IsModified = false;


    }

    public void UpdateEmail(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        _context.Entry(doctor).Property(d => d.Identification).IsModified = false;
        _context.Entry(doctor).Property(d => d.Password).IsModified = false;
    }

    public void UpdatePassword(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        _context.Entry(doctor).Property(d => d.Identification).IsModified = false;
        _context.Entry(doctor).Property(d => d.Email).IsModified = false;
    }
}
