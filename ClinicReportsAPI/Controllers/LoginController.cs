using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicReportsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _service;

    public LoginController(ILoginService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDTO requestDto)
    {
        var response = await _service.Login(requestDto);

        return Ok(response);
    }

}
