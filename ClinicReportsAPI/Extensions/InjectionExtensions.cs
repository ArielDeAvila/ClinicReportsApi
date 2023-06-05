using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Validations.Register;
using FluentValidation;

namespace ClinicReportsAPI.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionServices(this IServiceCollection services)
    {
        services.AddScoped<IHospitalService, HospitalService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IVerifyEmailService, VerifyEmailService>();

        services.AddScoped<IValidator<HospitalRegisterDTO>, HospitalRegisterValidation>();
        services.AddScoped<IValidator<DoctorRegisterDTO>, RegisterDoctorValidation>();
        services.AddScoped<IValidator<PatientRegisterDTO>, PatientRegisterValidation>();
        
        return services;
    }
}
