using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;

namespace ClinicReportsAPI.Services;

public class HospitalService : IHospitalService
{
    private readonly IUnitOfWork _unitOfWork;

    public HospitalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<IEnumerable<HospitalDTO>>> GetAll()
    {
        var response = new BaseResponse<IEnumerable<HospitalDTO>>();  

        var hospitals = await _unitOfWork.HospitalRepository.GetAll();

        //var hospitalsDTO = new List<HospitalDTO>();
        //foreach (var hospital in hospitals) hospitalsDTO.Add((HospitalDTO)hospital);

        var hospitalsDTO = (List<HospitalDTO>)hospitals;

        if (hospitals is not null)
        {
            response.Success = true;
            response.Data = hospitalsDTO;
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

        var hospitalsDTO = new List<HospitalNameDTO>();
        foreach (var hospital in hospitals) hospitalsDTO.Add((HospitalNameDTO)hospital);

        if (hospitals is not null)
        {
            response.Success = true;
            response.Data = hospitalsDTO;
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

        var hospital = (Hospital)hospitalDTO;

        var medicalServices = ConvertList.ToListMedicalService(hospitalDTO.Services);

        //TODO: encriptar contraseña
        _unitOfWork.HospitalRepository.Create(hospital,medicalServices);

        var created = await _unitOfWork.CommitAsync();

        response.Data = created > 0;

        if(response.Data)
        {
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
}
