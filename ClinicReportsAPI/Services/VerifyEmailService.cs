using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClinicReportsAPI.Services;

public class VerifyEmailService : IVerifyEmailService
{
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<bool>> VerifyEmail(VerifyEmailDTO dto)
    {
        var response = new BaseResponse<bool>();

        BaseUser account;

        switch (dto.AccountType)
        {
            case (int)AccountType.Hospital:

                account = await _unitOfWork.HospitalRepository
                    .GetHospital(h => h.VerifyToken.Equals(dto.VerifyToken)); break;

            case (int)AccountType.Patient:

                account = await _unitOfWork.PatientRepository
                    .GetPatient(p => p.VerifyToken.Equals(dto.VerifyToken)); break;

            case (int)AccountType.Doctor:

                account = await _unitOfWork.DoctorRepository
                    .GetDoctor(p => p.VerifyToken.Equals(dto.VerifyToken)); break;

            default:

                return new BaseResponse<bool>
                {
                    Success = false,
                    Message = ReplyMessage.MESSAGE_FAILED
                };
        }

        if (account is null)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_INVALID_TOKEN;

            return response;
        }

        if(account.VerifiedAt is not null)
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_ALREADY_VERIFIED;

            return response;
        }

        account.VerifiedAt = DateTime.Now;

        _unitOfWork.Update(account);
        var verified = await _unitOfWork.CommitAsync();

        response.Data = verified > 0;

        if (response.Data)
        {
            response.Success = true;
            response.Message = ReplyMessage.MESSAGE_VERIFIED;
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_FAILED;
        }

        return response;
    }
}
