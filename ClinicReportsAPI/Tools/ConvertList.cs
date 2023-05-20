using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;

namespace ClinicReportsAPI.Tools;

public static class ConvertList
{
    public static List<MedicalServiceDTO> ToListMedicalServiceDTO(List<MedicalService> medicalServices)
    {
        var medicalServicesDTO = new List<MedicalServiceDTO>();

        foreach (var ms in medicalServices)
        {
            medicalServicesDTO.Add((MedicalServiceDTO)ms);
        }

        return medicalServicesDTO;
    }

    public static List<MedicalService> ToListMedicalService(List<MedicalServiceDTO> medicalServicesDTO)
    {
        var medicalServices = new List<MedicalService>();

        foreach (var ms in medicalServicesDTO)
        {
            medicalServices.Add((MedicalService)ms);
        }

        return medicalServices;
    }
}
