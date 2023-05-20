namespace ClinicReportsAPI.Data.Entities;

public class Patient : BaseUser
{
    public DateTime BirthDate { get; set; }

    public int HospitalId { get; set; }
    public Hospital Hospital { get; set; } = null!;

    public ICollection<Report> Reports { get; set; } = new List<Report>();
}
