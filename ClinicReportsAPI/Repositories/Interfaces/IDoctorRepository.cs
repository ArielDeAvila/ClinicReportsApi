using ClinicReportsAPI.Data.Entities;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories.Interfaces;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor> GetDoctor(Expression<Func<Doctor, bool>> expression);
    void UpdatePassword(Doctor doctor);
    void UpdateEmail(Doctor doctor);
}
