namespace ClinicReportsAPI.Data.Entities;

public class Hospital : BaseUser
{
    public ICollection<HospitalMedicalService> HospitalServices { get; set; } = new List<HospitalMedicalService>();

    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public ICollection<Report> Reports { get; set; } = new List<Report>();

    public ICollection<Patient> Patients { get; set;} = new List<Patient>();
}
