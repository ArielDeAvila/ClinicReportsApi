using ClinicReportsAPI.DTOs.Register;
using FluentValidation;

namespace ClinicReportsAPI.Validations.Register;

public class RegisterDoctorValidation : AbstractValidator<DoctorRegisterDTO>
{
    public RegisterDoctorValidation()
    {
        RuleFor(doc => doc.Email).EmailAddress().NotEmpty().NotNull();
        RuleFor(doc => doc.Address).NotEmpty().NotNull();
        RuleFor(doc => doc.PhoneNumber).NotEmpty().NotNull();
        RuleFor(doc => doc.BirthDate).NotEmpty().NotNull();
        RuleFor(doc => doc.Identification).NotEmpty().NotNull();
        RuleFor(doc => doc.MedicalSpecialty).NotEmpty().NotNull();
        RuleFor(doc => doc.Name).NotEmpty().NotNull();
    }
}

