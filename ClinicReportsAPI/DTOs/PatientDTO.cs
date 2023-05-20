using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs.Name;

namespace ClinicReportsAPI.DTOs;

public class PatientDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public HospitalNameDTO Hospital { get; set; } = null!; 

    public static explicit operator PatientDTO(Patient patient)
    {
        if (patient is not null) return new PatientDTO
        {
            Id = patient.Id,
            Name = patient.Name,
            Email = patient.Email,
            Identification = patient.Identification,
            PhoneNumber = patient.PhoneNumber,
            Address = patient.Address,
            BirthDate = patient.BirthDate,
            Hospital = (HospitalNameDTO)patient.Hospital
        };

        return default!;
    }

    public static explicit operator Patient(PatientDTO patientDto)
    {
        if (patientDto is not null) return new Patient
        {
            Id = patientDto.Id,
            Name = patientDto.Name,
            Email = patientDto.Email,
            Identification = patientDto.Identification,
            PhoneNumber = patientDto.PhoneNumber,
            Address = patientDto.Address,
            BirthDate = patientDto.BirthDate,
            HospitalId = patientDto.Hospital.Id
        };

        return default!;
    }
}
