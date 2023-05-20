namespace ClinicReportsAPI.Data.Entities;

public class MedicalService : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!; 

    public ICollection<HospitalMedicalService> HospitalServices { get; set; } = new List<HospitalMedicalService>();
}
