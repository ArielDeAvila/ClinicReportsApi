namespace ClinicReportsAPI.Data.Entities;

public class Report : BaseEntity
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string Observation { get; set; } = null!;

    public string Treatment { get; set; } = null!;

}
