using ClinicReportsAPI.Data.Entities;

namespace ClinicReportsAPI.Repositories.Interfaces;

public interface IHospitalRepository : IGenericRepository<Hospital>
{
    void Create(Hospital hospital, List<MedicalService> medicalServices);
    void Update(Hospital hospital, List<MedicalService> medicalServices);
}
