using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicReportsAPI.Controllers;

//TODO: Las rutas no deben ser accedidas por usuarios que no sean de tipo Hospital y Doctor
[Route("api/[controller]")]
[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _service;

    public DoctorController(IDoctorService service)
    {
        _service = service;
    }

    [HttpGet("Names")]
    public async Task<IActionResult> GetAllNames()
    {
        var response = await _service.GetAllNames();

        return Ok(response);    
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(DoctorRegisterDTO doctorDto)
    {
        var response = await _service.Create(doctorDto);

        return Ok(response);
    }

    [HttpPut("Edit")]
    public async Task<IActionResult> Update(DoctorDTO doctorDTO)
    {
        var response = await _service.Update(doctorDTO);

        return Ok(response);
    }

    [HttpDelete("Remove/{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var response = await _service.Remove(id); 
        
        return Ok(response);
    }
}
