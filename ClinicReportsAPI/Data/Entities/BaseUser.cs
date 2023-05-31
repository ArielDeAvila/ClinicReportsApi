namespace ClinicReportsAPI.Data.Entities;

public class BaseUser : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string VerifyToken { get; set; } = null!;
    public DateTime? VerifiedAt { get; set; } = null;    

}
