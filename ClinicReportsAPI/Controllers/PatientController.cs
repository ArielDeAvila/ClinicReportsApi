using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Extensions;
using ClinicReportsAPI.Services.Interfaces;
using ClinicReportsAPI.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ClinicReportsAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;
    private readonly IVerifyEmailService _verifyService;

    public PatientController(IPatientService service, IVerifyEmailService verifyService)
    {
        _service = service;
        _verifyService = verifyService;
    }

    [AuthorizeRole("Hospital")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();

        return Ok(response);
    }

    [AuthorizeRole("Hospital", "Doctor")]
    [HttpGet("{id:string}")]
    public async Task<IActionResult> GetByIdentification(string id)
    {
        var response = await _service.GetByIdentification(id);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Create([FromBody] PatientRegisterDTO patientDTO)
    {
        var response = await _service.Create(patientDTO);

        return Ok(response);
    }

    //TODO: validar
    [AuthorizeRole("Patient", "Hospital")]
    [HttpPut("Edit")]
    public async Task<IActionResult> Update([FromBody] PatientDTO patientDTO)
    {
        var response = await _service.Update(patientDTO);

        return Ok(response);
    }


    [AuthorizeRole("Hospital")]
    [HttpDelete("Remove/{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var reponse = await _service.Remove(id);

        return Ok(reponse);
    }

    [AuthorizeRole("Patient", "Hospital")]
    [HttpPut("ChangeEmail")]
    public async Task<IActionResult> UpdatePassword(UpdateEmailDTO requestDto)
    {
        var response = await _service.UpdateEmail(requestDto);

        return Ok(response);
    }

    [AuthorizeRole("Patient", "Hospital")]
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

    [AllowAnonymous]
    [HttpPost("email-validation")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailDTO dto)
    {
        var response = await _verifyService.VerifyEmail(dto);

        return Ok(response);
    }


}
