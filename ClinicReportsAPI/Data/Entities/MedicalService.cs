namespace ClinicReportsAPI.Data.Entities;

public class MedicalService : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;

    
}
