using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using FluentValidation;
using BC = BCrypt.Net.BCrypt;

namespace ClinicReportsAPI.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IValidator<DoctorRegisterDTO> _validator;

    public DoctorService(IUnitOfWork unitOfWork, IEmailService emailService, IValidator<DoctorRegisterDTO> validator)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _validator = validator;
    }

    public async Task<BaseResponse<IEnumerable<DoctorDTO>>> GetAll()
    {
        var response = new BaseResponse<IEnumerable<DoctorDTO>>();

        var doctors = await _unitOfWork.DoctorRepository.GetAll();

        if(doctors.Any())
        {
            response.Success = true;
            response.Data = (List<DoctorDTO>)doctors;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<IEnumerable<DoctorNameDTO>>> GetAllNames()
    {
        var response = new BaseResponse<IEnumerable<DoctorNameDTO>>();

        var doctors = await _unitOfWork.DoctorRepository.GetAll();

        if(doctors.Any())
        {
            response.Success = true;
            response.Data = (List<DoctorNameDTO>)doctors;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<DoctorDTO>> GetById(int id)
    {
        var response = new BaseResponse<DoctorDTO>();

        var doctor = await _unitOfWork.DoctorRepository.GetDoctor(d => d.Id.Equals(id) && d.AuditDateDelete == null);

        if(doctor is not null)
        {
            response.Success = true;
            response.Data = (DoctorDTO)doctor;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Create(DoctorRegisterDTO dto)
    {
        var response = new BaseResponse<bool>();
        var validate = await _validator.ValidateAsync(dto);

        if (!validate.IsValid)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_VALIDATE;
            response.Errors = validate.Errors;

            return response;
        }

        var existingDoctor = await _unitOfWork.DoctorRepository
            .GetDoctor(d => d.Email.Equals(dto.Email) || d.Identification.Equals(dto.Identification));

        if(existingDoctor is not null)
        {
            if (existingDoctor.Email.Equals(dto.Email))
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_EXISTS_EMAIL;

                return response;
            }
            else if(existingDoctor.Identification.Equals(dto.Identification))
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_EXISTS_IDENTIFICATION;

                return response;
            }
        }

        var doctor = (Doctor)dto;
        doctor.Password = TokenGenerator.GetRandomString(8); 
        doctor.VerifyToken = await VerifyToken(); 

        _unitOfWork.DoctorRepository.Create(doctor);

        var created = await _unitOfWork.CommitAsync();

        response.Data = created > 0;

        if (response.Data)
        {
            _emailService.SendEmail(doctor.Email, doctor.VerifyToken, "Doctor");

            response.Success = true;
            response.Message = ReplyMessage.MESSAGE_SAVE;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Update(DoctorDTO dto)
    {
        var response = new BaseResponse<bool>();

        _unitOfWork.DoctorRepository.Update((Doctor)dto);

        var updated = await _unitOfWork.CommitAsync();

        if(updated > 0)
        {
            response.Success = true;
            response.Data = true;
            response.Message = ReplyMessage.MESSAGE_UPDATE;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> UpdateEmail(UpdateEmailDTO emailDto)
    {
        var response = new BaseResponse<bool>();

        var existingDoctor = await _unitOfWork.DoctorRepository.GetDoctor(d => d.Email.Equals(emailDto.NewEmail));

        if (existingDoctor is not null)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_EXISTS_EMAIL;

            return response;
        }

        var doctor = await _unitOfWork.DoctorRepository.GetDoctor(d => d.Email.Equals(emailDto.OldEmail));

        doctor.Email = emailDto.NewEmail;

        _unitOfWork.DoctorRepository.UpdateEmail(doctor);

        var updated = await _unitOfWork.CommitAsync();

        response.Data = updated > 0;

        if (response.Data)
        {
            response.Success = true;
            response.Message = ReplyMessage.MESSAGE_UPDATE;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> UpdatePassword(UpdatePasswordDTO passwordDto, int id)
    {
        var response = new BaseResponse<bool>();

        var doctor = await _unitOfWork.DoctorRepository.GetDoctor(d => d.Id.Equals(id));

        if(doctor.IsFirstLogin) doctor.IsFirstLogin = false;

        if(!BC.Verify(passwordDto.OldPassword, doctor.Password))
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_PASSWORD_ERROR;

            return response;
        }

        doctor.Password = BC.HashPassword(passwordDto.NewPassword);

        _unitOfWork.DoctorRepository.UpdatePassword(doctor);

        var update = await _unitOfWork.CommitAsync();

        response.Data = update > 0;

        if (response.Data)
        {
            response.Success = true;
            response.Message = ReplyMessage.MESSAGE_UPDATE;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Remove(int id)
    {
        var response = new BaseResponse<bool>();

        _unitOfWork.DoctorRepository.Remove(id);

        var removed = await _unitOfWork.CommitAsync();

        if(removed > 0)
        {
            response.Success = true;
            response.Data = true;
            response.Message = ReplyMessage.MESSAGE_DELETE;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }

    public async void SendCredentials(VerifyEmailDTO dto)
    {
        var account = await _unitOfWork.DoctorRepository
            .GetDoctor(d => d.VerifyToken.Equals(dto.VerifyToken) && d.AuditDateDelete == null);

        if (account is not null) _emailService.SendCredentials(account);
    }

    private async Task<string> VerifyToken()
    {
        string token = TokenGenerator.GenerateRandomToken();

        var exists = await _unitOfWork.DoctorRepository.GetDoctor(p => p.VerifyToken.Equals(token));

        if (exists is null)
        {
            return token;
        }
        else
        {
            return await VerifyToken();
        }

    }


}
