using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.DTOs.Register;

public class HospitalRegisterDTO
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<MedicalServiceDTO> Services { get; set; } = null!;


    public static explicit operator Hospital(HospitalRegisterDTO hospital)
    {
        if (hospital is not null) return new Hospital
        {
            
            Name = hospital.Name,
            Email = hospital.Email,
            Password = hospital.Password,
            Identification = hospital.Identification,
            PhoneNumber = hospital.PhoneNumber,
            Address = hospital.Address

        };

        return default!;
    }
}
