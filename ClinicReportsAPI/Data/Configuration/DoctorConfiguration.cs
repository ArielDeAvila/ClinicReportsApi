using ClinicReportsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicReportsAPI.Data.Configuration;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id).HasName("PK_Doctor");

        builder.HasOne(d => d.Hospital).WithMany(h => h.Doctors)
            .HasForeignKey(d => d.HospitalId)
            .HasConstraintName("FK_Doctor_Hospital");

        builder.HasIndex(d => d.Email).IsUnique();


    }
}
