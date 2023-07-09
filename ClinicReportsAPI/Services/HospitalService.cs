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

public class HospitalService : IHospitalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IValidator<HospitalRegisterDTO> _validator;

    public HospitalService(IUnitOfWork unitOfWork, IEmailService emailService, IValidator<HospitalRegisterDTO> validator)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _validator = validator;
    }

    public async Task<BaseResponse<IEnumerable<HospitalDTO>>> GetAll()
    {
        var response = new BaseResponse<IEnumerable<HospitalDTO>>();  

        var hospitals = await _unitOfWork.HospitalRepository.GetAll();


        if (hospitals is not null)
        {
            var data = new List<HospitalDTO>();
                foreach (var hospital in hospitals) data.Add((HospitalDTO)hospital);

            response.Success = true;
            response.Data = data;
            response.Message = ReplyMessage.MESSAGE_QUERY;

        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<IEnumerable<HospitalNameDTO>>> GetAllNames()
    {
        var response = new BaseResponse<IEnumerable<HospitalNameDTO>>();

        var hospitals = await _unitOfWork.HospitalRepository.GetAll();

        if (hospitals is not null)
        {
            var data = new List<HospitalNameDTO>();
                foreach (var hospital in hospitals) data.Add((HospitalNameDTO)hospital);

            response.Success = true;
            response.Data = data;
            response.Message = ReplyMessage.MESSAGE_QUERY;

        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<HospitalDTO>> GetById(int id)
    {
        var response = new BaseResponse<HospitalDTO>();

        var hospital = await _unitOfWork.HospitalRepository.GetById(id);

        if(hospital is not null)
        {
            response.Success = true;
            response.Data = (HospitalDTO)hospital;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;

    }

    public async Task<BaseResponse<bool>> Create(HospitalRegisterDTO hospitalDTO)
    {
        var response = new BaseResponse<bool>();
        var validation = await _validator.ValidateAsync(hospitalDTO);

        if (!validation.IsValid)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_VALIDATE;
            response.Errors = validation.Errors;

            return response;
        }

        var existingHospital = await _unitOfWork.HospitalRepository
            .GetHospital(d => d.Email.Equals(hospitalDTO.Email) || d.Identification.Equals(hospitalDTO.Identification));

        if (existingHospital is not null)
        {
            if (existingHospital.Email.Equals(hospitalDTO.Email))
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_EXISTS_EMAIL;

                return response;
            }
            else if (existingHospital.Identification.Equals(hospitalDTO.Identification))
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_EXISTS_IDENTIFICATION;

                return response;
            }
        }

        var hospital = (Hospital)hospitalDTO;

        var medicalServices = ConvertList.ToListMedicalService(hospitalDTO.Services);

        hospital.Password = BC.HashPassword(hospital.Password);
        hospital.VerifyToken = await VerifyToken();

        _unitOfWork.HospitalRepository.Create(hospital,medicalServices);

        var created = await _unitOfWork.CommitAsync();

        response.Data = created > 0;

        if(response.Data)
        {
            _emailService.SendEmail(hospital.Email, hospital.VerifyToken, "Hospital");

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

    public async Task<BaseResponse<bool>> Remove(int id)
    {
        var response = new BaseResponse<bool>();

        //comprobamos que el registro exista
        var exists = await GetById(id);

        if (!exists.Success)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;

            return response;
        }

        _unitOfWork.HospitalRepository.Remove(id);

        var deleted = await _unitOfWork.CommitAsync();

        response.Data = deleted > 0;

        if (response.Data)
        {
            response.Success = true;
            response.Message = ReplyMessage.MESSAGE_DELETE;
        }
        else
        {
            response.Success= false;
            response.Message= ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Update(HospitalDTO hospitalDTO)
    {
        var response = new BaseResponse<bool>();

        //comprobamos que el registro exista
        var exists = await GetById(hospitalDTO.Id);

        if (!exists.Success)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;

            return response;
        }

        var hospital = (Hospital)hospitalDTO;

        var medicalServices = ConvertList.ToListMedicalService(hospitalDTO.Services);

        _unitOfWork.HospitalRepository.Update(hospital, medicalServices);

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

    public async Task<BaseResponse<bool>> UpdateEmail(UpdateEmailDTO emailDto)
    {
        var response = new BaseResponse<bool>();

        var existingHospital = await _unitOfWork.HospitalRepository.GetHospital(d => d.Email.Equals(emailDto.NewEmail));

        if (existingHospital is not null)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_EXISTS;

            return response;
        }

        var hospital = await _unitOfWork.HospitalRepository.GetHospital(d => d.Email.Equals(emailDto.OldEmail));

        hospital.Email = emailDto.NewEmail;

        _unitOfWork.HospitalRepository.UpdateEmail(hospital);

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

        var hospital = await _unitOfWork.HospitalRepository.GetHospital(d => d.Id.Equals(id));

        if (!BC.Verify(passwordDto.OldPassword, hospital.Password))
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_PASSWORD_ERROR;

            return response;
        }

        hospital.Password = BC.HashPassword(passwordDto.NewPassword);

        _unitOfWork.HospitalRepository.UpdatePassword(hospital);

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

    private async Task<string> VerifyToken()
    {
        string token = TokenGenerator.GenerateRandomToken();

        var exists = await _unitOfWork.HospitalRepository.GetHospital(p => p.VerifyToken.Equals(token));

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
