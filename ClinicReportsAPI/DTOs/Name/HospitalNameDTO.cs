using ClinicReportsAPI.Data.Entities;

namespace ClinicReportsAPI.DTOs.Name;

public class HospitalNameDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public static explicit operator HospitalNameDTO(Hospital hospital)
    {
        if (hospital is not null)
        {
            return new HospitalNameDTO()
            {
                Id = hospital.Id,
                Name = hospital.Name,
            };
        }

        return default!;
    }

}
