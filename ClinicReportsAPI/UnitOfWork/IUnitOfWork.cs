using ClinicReportsAPI.Repositories.Interfaces;

namespace ClinicReportsAPI.UnitOfWork;

public interface IUnitOfWork
{
    IHospitalRepository HospitalRepository { get; }
    IReportRepository ReportRepository { get; }
    IPatientRepository PatientRepository { get; }
    IDoctorRepository DoctorRepository { get; }

    void Commit();
    void Dispose();
    Task<int> CommitAsync();
    Task DisposeAsync();
}
