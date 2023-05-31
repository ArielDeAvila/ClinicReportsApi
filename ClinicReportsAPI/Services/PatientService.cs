using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using BC = BCrypt.Net.BCrypt;

namespace ClinicReportsAPI.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public PatientService(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<BaseResponse<IEnumerable<PatientDTO>>> GetAll()
    {
        var response = new BaseResponse<IEnumerable<PatientDTO>>(); 

        var patients = await _unitOfWork.PatientRepository.GetAll();

        if(patients is not null)
        {
            response.Success = true;
            response.Data = (List<PatientDTO>)patients;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<PatientDTO>> GetByIdentification(string id)
    {
        var response = new BaseResponse<PatientDTO>();

        var patient = await _unitOfWork.PatientRepository.GetByIdentification(id);

        if(patient is not null)
        {
            response.Success = true;
            response.Data = (PatientDTO)patient;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Create(PatientRegisterDTO patientDTO)
    {
        var response = new BaseResponse<bool>();

        var existingDoctor = await _unitOfWork.DoctorRepository
            .GetDoctor(d => d.Email.Equals(patientDTO.Email) || d.Identification.Equals(patientDTO.Identification));

        if (existingDoctor is not null)
        {
            if (existingDoctor.Email.Equals(patientDTO.Email))
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_EXISTS_EMAIL;

                return response;
            }
            else if (existingDoctor.Identification.Equals(patientDTO.Identification))
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_EXISTS_IDENTIFICATION;

                return response;
            }
        }

        var patient = (Patient)patientDTO;

        patient.Password = BC.HashPassword(patientDTO.Password);
        patient.VerifyToken = await VerifyToken();

        _unitOfWork.PatientRepository.Create(patient);

        var created = await _unitOfWork.CommitAsync();

        response.Data = created > 0;
        

        if(response.Data)
        {
            _emailService.SendEmail(patient.Email, patient.VerifyToken, "Patient");

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

    public async Task<BaseResponse<bool>> Update(PatientDTO patientDTO)
    {
        var response = new BaseResponse<bool>();

        _unitOfWork.PatientRepository.Update((Patient)patientDTO);

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

        var existingPatient = await _unitOfWork.PatientRepository.GetPatient(d => d.Email.Equals(emailDto.NewEmail));

        if (existingPatient is not null)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_EXISTS;

            return response;
        }

        var patient = await _unitOfWork.PatientRepository.GetPatient(d => d.Email.Equals(emailDto.OldEmail));

        patient.Email = emailDto.NewEmail;

        _unitOfWork.PatientRepository.UpdateEmail(patient);

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

        var patient = await _unitOfWork.PatientRepository.GetPatient(d => d.Id.Equals(id));

        if (!BC.Verify(passwordDto.OldPassword, patient.Password))
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_PASSWORD_ERROR;

            return response;
        }

        patient.Password = BC.HashPassword(passwordDto.NewPassword);

        _unitOfWork.PatientRepository.UpdatePassword(patient);

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

        _unitOfWork.PatientRepository.Remove(id);

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

    private async Task<string> VerifyToken()
    {
        string token = TokenGenerator.GenerateRandomToken();

        var exists = await _unitOfWork.PatientRepository.GetPatient(p => p.VerifyToken.Equals(token));

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
