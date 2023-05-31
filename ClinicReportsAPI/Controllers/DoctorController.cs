using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Extensions;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ClinicReportsAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _service;
    private readonly IVerifyEmailService _verifyService;

    public DoctorController(IDoctorService service, IVerifyEmailService verifyService)
    {
        _service = service;
        _verifyService = verifyService;
    }

    [AllowAnonymous]
    [HttpGet("Names")]
    public async Task<IActionResult> GetAllNames()
    {
        var response = await _service.GetAllNames();

        return Ok(response);    
    }

    [AuthorizeRole("Hospital")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();

        return Ok(response);
    }

    [AuthorizeRole("Hospital")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetById(id);

        return Ok(response);
    }

    [AuthorizeRole("Hospital")]
    [HttpPost("Register")]
    public async Task<IActionResult> Register(DoctorRegisterDTO doctorDto)
    {
        var response = await _service.Create(doctorDto);

        return Ok(response);
    }

    [AuthorizeRole("Hospital", "Doctor")]
    [HttpPut("Edit")]
    public async Task<IActionResult> Update(DoctorDTO doctorDTO)
    {
        var response = await _service.Update(doctorDTO);

        return Ok(response);
    }

    [AuthorizeRole("Hospital")]
    [HttpPut("ChangeEmail")]
    public async Task<IActionResult> UpdatePassword(UpdateEmailDTO requestDto)
    {
        var response = await _service.UpdateEmail(requestDto);

        return Ok(response);
    }

    [AuthorizeRole("Hospital", "Doctor")]
    [HttpPut("ChangePassword")]
    public async Task<IActionResult> UpdatePasswotd(UpdatePasswordDTO requestDto)
    {
        var uniqueNameClaim = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);

        if (uniqueNameClaim == null) return BadRequest(new BaseResponse<bool>
        {
            Success = false,
            Message = ReplyMessage.MESSAGE_FAILED
        });

        int id = int.Parse(uniqueNameClaim.Value);

        var response = await _service.UpdatePassword(requestDto, id);

        return Ok(response);

    }

    [AuthorizeRole("Hospital")]
    [HttpDelete("Remove/{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var response = await _service.Remove(id); 
        
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("email-validation")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailDTO dto)
    {
        var response = await _verifyService.VerifyEmail(dto);

        if(response.Data) _service.SendCredentials(dto);

        return Ok(response);
    }
}
