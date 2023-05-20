using ClinicReportsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicReportsAPI.Data.Configuration;

public class HospitalMedicalServiceConfiguration : IEntityTypeConfiguration<HospitalMedicalService>
{
    public void Configure(EntityTypeBuilder<HospitalMedicalService> builder)
    {
        builder.HasKey(hs => new {hs.ServiceId,hs.HospitalId});

        builder.HasOne(hs => hs.Service).WithMany(ms => ms.HospitalServices)
            .HasForeignKey(hs => hs.ServiceId)
            .HasConstraintName("FK_Service_HospitalService");

        builder.HasOne(hs => hs.Hospital).WithMany(h => h.HospitalServices)
            .HasForeignKey(hs => hs.HospitalId)
            .HasConstraintName("FK_Hospital_HospitalService");


    }
}
