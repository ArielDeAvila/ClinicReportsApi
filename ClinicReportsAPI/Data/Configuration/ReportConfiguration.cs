using ClinicReportsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicReportsAPI.Data.Configuration;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(r => r.Id).HasName("FK_Report");

        builder.HasOne(r => r.Doctor).WithMany(d => d.Reports)
            .HasForeignKey(r => r.DoctorId)
            .HasConstraintName("FK_Report_Doctor");

        builder.HasOne(r => r.Patient).WithMany(p => p.Reports)
            .HasForeignKey(r => r.PatientId)
            .HasConstraintName("FK_Report_Patient");

    }
}
