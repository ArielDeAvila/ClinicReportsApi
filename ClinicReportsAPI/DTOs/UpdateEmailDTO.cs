namespace ClinicReportsAPI.DTOs;

public class UpdateEmailDTO
{
    public string OldEmail { get; set; } = null!;
    public string NewEmail { get; set; } = null!;
}
