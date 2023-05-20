using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicReportsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HospitalController : ControllerBase
{
    private readonly IHospitalService _service;

    public HospitalController(IHospitalService service)
    {
        _service = service;
    }

    //TODO: Mirar si realmente es necesaria esta ruta
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();

        return Ok(response);
    }

    [HttpGet("Names")]
    public async Task<IActionResult> GetAllNames()
    {
        var response = await _service.GetAllNames();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetById(id);

        return Ok(response);
    }

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

    //TODO: agregar validación
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

    //TODO: agredar validación
    [HttpDelete("Delete/{id:int}")]
    public async Task<IActionResult> DeleteHospital(int id)
    {
        var response = await _service.Remove(id);

        return Ok(response);
    }

    //TODO: crear un login

}
