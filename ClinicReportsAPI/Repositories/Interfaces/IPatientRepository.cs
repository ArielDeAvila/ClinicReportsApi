using ClinicReportsAPI.Data.Entities;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> GetPatient(Expression<Func<Patient, bool>> expression);
        Task<Patient> GetByIdentification(string identification);
        void UpdatePassword(Patient patient);
        void UpdateEmail(Patient patient);
    }
}
