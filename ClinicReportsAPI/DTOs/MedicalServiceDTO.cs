using ClinicReportsAPI.Data.Entities;

namespace ClinicReportsAPI.DTOs;

public class MedicalServiceDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;


    public static explicit operator MedicalServiceDTO(MedicalService medicalService)
    {
        if (medicalService is not null) return new MedicalServiceDTO()
        {
            Id = medicalService.Id,
            Name = medicalService.Name
        };
                
        return default!;
    }

    public static explicit operator MedicalService(MedicalServiceDTO medicalServiceDTO)
    {
        if (medicalServiceDTO is not null) return new MedicalService()
        {
            Id = medicalServiceDTO.Id,
            Name = medicalServiceDTO.Name
        };

        return default!;
    }
}
