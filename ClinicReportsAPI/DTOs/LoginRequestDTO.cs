namespace ClinicReportsAPI.DTOs;

public class LoginRequestDTO
{
    public string Identification { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int AccountType { get; set; }
}
