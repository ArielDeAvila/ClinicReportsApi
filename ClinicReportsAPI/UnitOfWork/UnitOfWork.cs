using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories;
using ClinicReportsAPI.Repositories.Interfaces;

namespace ClinicReportsAPI.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SystemReportContext _context;
    private IHospitalRepository _hospitalRepository = null!;
    private IReportRepository _reportRepository = null!;
    private IPatientRepository _petientRepository = null!;
    private IDoctorRepository _doctorRepository = null!;

    public UnitOfWork(SystemReportContext context)
    {
        _context = context;
    }

    public IHospitalRepository HospitalRepository
    {
        get
        {
            return _hospitalRepository ??= new HospitalRepository(_context);
        }
    }

    public IReportRepository ReportRepository
    {
        get
        {
            return _reportRepository ??= new ReportRepository(_context);
        }
    }

    public IPatientRepository PatientRepository
    {
        get
        {
            return _petientRepository ??= new PatientRepository(_context);
        }
    }

    public IDoctorRepository DoctorRepository
    {
        get
        {
            return _doctorRepository ??= new DoctorRepository(_context);   
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Update(BaseEntity entity) 
    {
        _context.Update(entity);
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();  
    }
}
