using ClinicReportsAPI.Data.Entities;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories.Interfaces;

public interface IHospitalRepository : IGenericRepository<Hospital>
{
    void Create(Hospital hospital, List<MedicalService> medicalServices);
    void Update(Hospital hospital, List<MedicalService> medicalServices);
    Task<Hospital> GetHospital(Expression<Func<Hospital, bool>> expression);
    void UpdatePassword(Hospital hospital);
    void UpdateEmail(Hospital hospital);
}
