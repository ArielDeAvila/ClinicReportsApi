namespace ClinicReportsAPI.Data.Entities;

public class HospitalMedicalService 
{
    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;

    public int ServiceId { get; set; }
    public MedicalService Service { get; set; } = null!;
}
