using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;

namespace ClinicReportsAPI.UnitOfWork;

public interface IUnitOfWork
{
    IHospitalRepository HospitalRepository { get; }
    IReportRepository ReportRepository { get; }
    IPatientRepository PatientRepository { get; }
    IDoctorRepository DoctorRepository { get; }

    void Update(BaseEntity entity);
    void Commit();
    void Dispose();
    Task<int> CommitAsync();
    Task DisposeAsync();
}
