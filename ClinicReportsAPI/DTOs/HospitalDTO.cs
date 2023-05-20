using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Tools;

namespace ClinicReportsAPI.DTOs;

public class HospitalDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Identification { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<MedicalServiceDTO> Services { get; set; } = null!;


    public static explicit operator HospitalDTO(Hospital hospital)
    {
        if (hospital is not null) return new HospitalDTO
        {
            Id = hospital.Id,
            Name = hospital.Name,
            Email = hospital.Email,
            Identification = hospital.Identification,
            PhoneNumber = hospital.PhoneNumber,
            Address = hospital.Address,
            Services = ConvertList.ToListMedicalServiceDTO(hospital.MedicalServices.ToList())

        };

        return default!;
    }

    public static explicit operator Hospital(HospitalDTO hospitalDTO)
    {
        if (hospitalDTO is not null) return new Hospital()
        {
            Id = hospitalDTO.Id,
            Name = hospitalDTO.Name,
            Email = hospitalDTO.Email,
            Identification = hospitalDTO.Identification,
            PhoneNumber = hospitalDTO.PhoneNumber,
            Address = hospitalDTO.Address,

        };

        return default!;
    }

}


