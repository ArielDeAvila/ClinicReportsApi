using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs.Name;

namespace ClinicReportsAPI.DTOs;

public class DoctorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string MedicalSpecialty { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public HospitalNameDTO Hospital { get; set; } = null!;

    public static explicit operator DoctorDTO(Doctor doctor)
    {
        if (doctor is not null) return new DoctorDTO
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Email = doctor.Email,
            Identification = doctor.Identification,
            PhoneNumber = doctor.PhoneNumber,
            Address = doctor.Address,
            MedicalSpecialty = doctor.MedicalSpecialty,
            BirthDate = doctor.Birthdate,
            Hospital = (HospitalNameDTO)doctor.Hospital
        };

        return default!;
    }

    public static explicit operator Doctor(DoctorDTO doctor)
    {
        if (doctor is not null) return new Doctor
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Email = doctor.Email,
            Identification = doctor.Identification,
            PhoneNumber = doctor.PhoneNumber,
            Address = doctor.Address,
            MedicalSpecialty = doctor.MedicalSpecialty,
            Birthdate = doctor.BirthDate,
            HospitalId = doctor.Hospital.Id
        };

        return default!;
    }

}
