using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs.Name;

namespace ClinicReportsAPI.DTOs.Register;

public class PatientRegisterDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public HospitalNameDTO Hospital { get; set; } = null!;

    public static explicit operator Patient(PatientRegisterDTO dto)
    {
        if (dto is not null) return new Patient
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            Identification = dto.Identification,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            BirthDate = dto.BirthDate,
            HospitalId = dto.Hospital.Id
        };

        return default!;
    }
}
