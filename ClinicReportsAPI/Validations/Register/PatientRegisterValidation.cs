using ClinicReportsAPI.DTOs.Register;
using FluentValidation;

namespace ClinicReportsAPI.Validations.Register;

public class PatientRegisterValidation : AbstractValidator<PatientRegisterDTO>
{
    public PatientRegisterValidation()
    {
        RuleFor(pat => pat.Name).NotEmpty().NotNull();
        RuleFor(pat => pat.Email).EmailAddress().NotEmpty().NotNull();
        RuleFor(pat => pat.Password).NotEmpty().NotNull();
        RuleFor(pat => pat.Identification).NotEmpty().NotNull();
        RuleFor(pat => pat.PhoneNumber).NotEmpty().NotNull();
        RuleFor(pat => pat.Address).NotEmpty().NotNull();
        RuleFor(pat => pat.BirthDate).NotEmpty().NotNull();
        RuleFor(pat => pat.Hospital).NotEmpty().NotNull();
    }
}
