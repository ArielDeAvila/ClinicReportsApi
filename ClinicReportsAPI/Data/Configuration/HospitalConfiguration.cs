using ClinicReportsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicReportsAPI.Data.Configuration;

public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.HasKey(h => h.Id).HasName("PK_Hospital");

        builder.HasIndex(h => h.Email).IsUnique();
    }
}
