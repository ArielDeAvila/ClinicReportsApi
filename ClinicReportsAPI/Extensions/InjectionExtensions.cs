﻿using ClinicReportsAPI.Services;
using ClinicReportsAPI.Services.Interfaces;

namespace ClinicReportsAPI.Extensions;

public static class InjectionExtensions
{
    public static IServiceCollection AddInjectionServices(this IServiceCollection services)
    {
        services.AddScoped<IHospitalService, HospitalService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        
        return services;
    }
}
