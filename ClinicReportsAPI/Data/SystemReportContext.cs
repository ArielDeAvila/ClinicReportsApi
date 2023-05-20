using ClinicReportsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClinicReportsAPI.Data;

public partial class SystemReportContext : DbContext
{
	public SystemReportContext(DbContextOptions<SystemReportContext> options) : base(options)
	{

	}

	public DbSet<Doctor> Doctors { get; set; }
	
	public DbSet<Hospital> Hospitals { get; set;}

	public DbSet<MedicalService> MedicalServices { get; set; }

	public DbSet<Report> Reports { get; set; }

	public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
