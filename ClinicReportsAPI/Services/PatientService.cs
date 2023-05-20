using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;

namespace ClinicReportsAPI.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

    public async Task<BaseResponse<PatientDTO>> GetById(int id)
    {
        var response = new BaseResponse<PatientDTO>();

        var patient = await _unitOfWork.PatientRepository.GetById(id);

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

        //TODO: encriptar contraseña
        _unitOfWork.PatientRepository.Create((Patient)patientDTO);

        var created = await _unitOfWork.CommitAsync();

        if(created > 0)
        {
            response.Success = true;
            response.Data = true;
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

}
