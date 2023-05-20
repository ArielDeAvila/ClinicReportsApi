namespace ClinicReportsAPI.Data.Entities;

public class Doctor : BaseUser
{
    public DateTime Birthdate { get; set; }

    public string MedicalSpecialty { get; set; } = null!;

    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;

    public ICollection<Report> Reports { get; set; } = new List<Report>();  
}
