using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Name;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;

namespace ClinicReportsAPI.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        var doctor = await _unitOfWork.DoctorRepository.GetById(id);

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
        //TODO: encriptar contraseña

        var response = new BaseResponse<bool>();

        _unitOfWork.DoctorRepository.Create((Doctor)dto);

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


}
