using ClinicReportsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicReportsAPI.Data.Configuration;

public class MedicalServiceConfiguration : IEntityTypeConfiguration<MedicalService>
{
    public void Configure(EntityTypeBuilder<MedicalService> builder)
    {
        builder.HasKey(ms => ms.Id).HasName("PK_MedicalService");

        builder.Property(ms => ms.AuditDateCreated).HasDefaultValue(DateTime.Now);

        builder.HasOne(ms => ms.Hospital).WithMany(h => h.MedicalServices)
               .HasForeignKey(ms => ms.HospitalId)
               .HasConstraintName("FK_Hospital_MedicalService");
    }
}
