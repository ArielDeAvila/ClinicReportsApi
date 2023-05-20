namespace ClinicReportsAPI.Data.Entities;

public class BaseEntity
{
    public int Id { get; set; }

    public DateTime? AuditDateCreated { get; set; }
    public DateTime? AuditDateUpdate { get; set; }
    public DateTime? AuditDateDelete { get; set; }
}
