﻿using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using ClinicReportsAPI.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace ClinicReportsAPI.Services;

public class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public LoginService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<BaseResponse<string>> Login(LoginRequestDTO requestDto)
    {
        BaseUser account;

        string rol = ((AccountType) requestDto.AccountType).ToString();

        switch (requestDto.AccountType) 
        {
            case (int)AccountType.Hospital:

                account = await _unitOfWork.HospitalRepository
                    .GetHospital(h => h.Identification.Equals(requestDto.Identification)); break;

            case (int)AccountType.Patient:

                account = await _unitOfWork.PatientRepository
                    .GetPatient(p => p.Identification.Equals(requestDto.Identification)); break;

            default:

                return new BaseResponse<string>
                {
                    Success = false,
                    Message = ReplyMessage.MESSAGE_FAILED
                };      
        }

        return GenerateResponse(account, rol, requestDto.Password);
        
    }

    public async Task<BaseResponse<string>> LoginDoctor(LoginRequestDTO requestDto)
    {
        var account = await _unitOfWork.DoctorRepository
                                .GetDoctor(d => d.Identification.Equals(requestDto.Identification));

        string rol = ((AccountType)requestDto.AccountType).ToString();

        if (account.IsFirstLogin)
        {
            return new BaseResponse<string>
            {
                Success = false,
                Data = $"{_configuration["Application:BaseUrl"]}/changepassword",
                Message = "Por favor, cambie su contraseña"
            };
        }

        return GenerateResponse(account, rol, requestDto.Password);
    }

    private BaseResponse<string> GenerateResponse(BaseUser account, string rol, string password)
    {
        var response = new BaseResponse<string>();

        if (account is not null && BC.Verify(password, account.Password))
        {
            if (account.VerifiedAt is not null)
            {
                response.Success = true;
                response.Data = GenerateToken(account, rol);
                response.Message = ReplyMessage.MESSAGE_TOKEN;
            }
            else
            {
                response.Success = false;
                response.Message = ReplyMessage.MESSAGE_UNVERIFIED;
            }
        }
        else
        {
            response.Success = false;
            response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
        }

        return response;

    }

    private string GenerateToken(BaseUser account, string rol)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId,account.Identification),
            new Claim(JwtRegisteredClaimNames.FamilyName, account.Name),
            new Claim(JwtRegisteredClaimNames.GivenName, account.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, rol)
        };

        var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims, 
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"]!)),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }

}
