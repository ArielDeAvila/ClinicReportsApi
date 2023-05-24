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
public class HospitalController : ControllerBase
{
    private readonly IHospitalService _service;

    public HospitalController(IHospitalService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet("Names")]
    public async Task<IActionResult> GetAllNames()
    {
        var response = await _service.GetAllNames();

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterHospital([FromBody] HospitalRegisterDTO hospitalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var response = await _service.Create(hospitalDTO);

        return Ok(response);

    }

    [AuthorizeRole("Hospital")]
    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditHospital([FromBody] HospitalDTO hospitalDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var response = await _service.Update(hospitalDTO); 
        
        return Ok(response);
    }

    [AuthorizeRole("Hospital")]
    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> DeleteHospital(int id)
    {
        var response = await _service.Remove(id);

        return Ok(response);
    }

    [AuthorizeRole("Hospital")]
    [HttpPut("ChangeEmail")]
    public async Task<IActionResult> UpdatePassword(UpdateEmailDTO requestDto)
    {
        var response = await _service.UpdateEmail(requestDto);

        return Ok(response);
    }

    [AuthorizeRole("Hospital")]
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

}
