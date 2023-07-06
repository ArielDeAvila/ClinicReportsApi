using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;

namespace ClinicReportsAPI.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> Create(ReportDTO reportDto)
    {
        var response = new BaseResponse<bool>();

        _unitOfWork.ReportRepository.Create((Report)reportDto);

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

    public async Task<BaseResponse<IEnumerable<ReportDTO>>> GetAllReportsByDoctor(int doctorId)
    {
        var response = new BaseResponse<IEnumerable<ReportDTO>>();

        var reports = await _unitOfWork.ReportRepository.GetAllReportsByDoctor(doctorId);

        

        if(reports is not null)
        {
            var data = new List<ReportDTO>();
                foreach (var report in reports) data.Add((ReportDTO)report);

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

    public async Task<BaseResponse<IEnumerable<ReportDTO>>> GetAllReportsByHospital(int hospitalId)
    {
        var response = new BaseResponse<IEnumerable<ReportDTO>>();

        var reports = await _unitOfWork.ReportRepository.GetAllReportsByHospital(hospitalId);

        if(reports is not null)
        {
            var data = new List<ReportDTO>();
                foreach (var report in reports) data.Add((ReportDTO)report);

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

    public async Task<BaseResponse<IEnumerable<ReportDTO>>> GetAllReportsByPatient(int patientId)
    {
        var response = new BaseResponse<IEnumerable<ReportDTO>>();

        var reports = await _unitOfWork.ReportRepository.GetAllReportsByPatient(patientId);

        if (reports is not null)
        {
            var data = new List<ReportDTO>();
                foreach (var report in reports) data.Add((ReportDTO)report);

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

    public async Task<BaseResponse<ReportDTO>> GetById(int id)
    {
        var response = new BaseResponse<ReportDTO>();

        var report = await _unitOfWork.ReportRepository.GetById(id);

        if(report is not null)
        {
            response.Success = true;
            response.Data = (ReportDTO)report;
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Update(ReportDTO reportDto)
    {
        var response = new BaseResponse<bool>();

        _unitOfWork.ReportRepository.Update((Report)reportDto);

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
            response.Message = ReplyMessage.MESSAGE_UPDATE;
        }

        return response;
    }

    public async Task<BaseResponse<bool>> Remove(int id)
    {
        var response = new BaseResponse<bool>();

        _unitOfWork.ReportRepository.Remove(id);

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

    public async Task<byte[]> DownloadReport(int id)
    {
        var report = await _unitOfWork.ReportRepository.GetReport(r => r.Id.Equals(id));

        var document = TemplateReport.GenerateDocument(report);

        return document;

    }

    
}
