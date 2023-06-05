using ClinicReportsAPI.DTOs.Register;
using FluentValidation;

namespace ClinicReportsAPI.Validations.Register;

public class HospitalRegisterValidation : AbstractValidator<HospitalRegisterDTO>
{
    public HospitalRegisterValidation()
    {
        RuleFor(hos => hos.Email).EmailAddress().NotNull().NotEmpty();
        RuleFor(hos => hos.Password).NotNull().NotEmpty();
        RuleFor(hos => hos.Name).NotNull().NotEmpty();
        RuleFor(hos => hos.Address).NotNull().NotEmpty();
        RuleFor(hos => hos.PhoneNumber).NotNull().NotEmpty();
        RuleFor(hos => hos.Services).NotNull().NotEmpty();
        RuleFor(hos => hos.Identification).NotNull().NotEmpty();
    }
}
