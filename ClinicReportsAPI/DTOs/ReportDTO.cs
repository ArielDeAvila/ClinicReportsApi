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

    public static explicit operator Report(ReportDTO report)
    {
        if (report is not null) return new Report()
        {
            Id = report.Id,
            Diagnosis = report.Diagnosis,
            Observation = report.Observation,
            Treatment = report.Treatment,
            HospitalId = report.Hospital.Id,
            DoctorId = report.Doctor.Id,
            PatientId = report.Patient.Id
        };

        return default!;
    }

}
