namespace ClinicReportsAPI.DTOs;

public class VerifyEmailDTO
{
    public int AccountType { get; set; }
    public string VerifyToken { get; set; } = null!;
}
