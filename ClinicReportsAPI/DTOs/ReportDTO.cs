using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs.Name;

namespace ClinicReportsAPI.DTOs;

public class ReportDTO
{
    public int Id { get; set; }

    public DoctorNameDTO Doctor { get; set; } = null!;

    public PatientDTO Patient { get; set; } = null!;

    public HospitalNameDTO Hospital = null!;

    public string Diagnosis { get; set; } = null!;

    public string Observation { get; set; } = null!;

    public string Treatment { get; set; } = null!;


    public static explicit operator ReportDTO(Report report)
    {
        if (report is not null) return new ReportDTO
        {
            Id = report.Id,
            Diagnosis = report.Diagnosis,
            Observation = report.Observation,
            Treatment = report.Treatment,
            Patient = (PatientDTO)report.Patient,
            Doctor = (DoctorNameDTO)report.Doctor,
            Hospital = (HospitalNameDTO)report.Hospital
        };

        return default!;
    }

    public static explicit operator Report(ReportDTO reportDto)
    {
        if (reportDto is not null) return new Report()
        {
            Id = reportDto.Id,
            Diagnosis = reportDto.Diagnosis,
            Observation = reportDto.Observation,
            Treatment = reportDto.Treatment,
            HospitalId = reportDto.Hospital.Id,
            DoctorId = reportDto.Doctor.Id,
            PatientId = reportDto.Patient.Id
        };

        return default!;
    }

}
