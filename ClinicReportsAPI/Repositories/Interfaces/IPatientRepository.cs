using ClinicReportsAPI.Data.Entities;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetPatient(Expression<Func<Patient, bool>> expression)
    }
}
