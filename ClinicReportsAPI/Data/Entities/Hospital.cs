namespace ClinicReportsAPI.Data.Entities;

public class Hospital : BaseUser
{
    public ICollection<MedicalService> MedicalServices { get; set; } = new List<MedicalService>();

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public ICollection<Report> Reports { get; set; } = new List<Report>();

    public ICollection<Patient> Patients { get; set;} = new List<Patient>();
}
