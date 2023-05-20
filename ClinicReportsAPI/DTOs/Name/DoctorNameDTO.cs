using ClinicReportsAPI.Data.Entities;

namespace ClinicReportsAPI.DTOs.Name;

public class DoctorNameDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string MedicalSpecialty { get; set; } = null!;

    public static explicit operator DoctorNameDTO(Doctor doctor)
    {
        if (doctor is not null)
        {
            return new DoctorNameDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                MedicalSpecialty = doctor.MedicalSpecialty
            };
        }

        return default!;
    }

}
