using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs.Name;

namespace ClinicReportsAPI.DTOs.Register;

public class DoctorRegisterDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string MedicalSpecialty { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public int HospitalId { get; set; } 

    public static explicit operator Doctor(DoctorRegisterDTO doctorDTO)
    {
        if (doctorDTO is not null) return new Doctor
        {
            Id = doctorDTO.Id,
            Name = doctorDTO.Name,
            Email = doctorDTO.Email,
            Password = doctorDTO.Password,
            Identification = doctorDTO.Identification,
            PhoneNumber = doctorDTO.PhoneNumber,
            Address = doctorDTO.Address,
            MedicalSpecialty = doctorDTO.MedicalSpecialty,
            Birthdate = doctorDTO.BirthDate,
            HospitalId = doctorDTO.HospitalId
        };

        return default!;
    }
}
