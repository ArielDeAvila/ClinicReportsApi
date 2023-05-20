using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.DTOs.Register;
using ClinicReportsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicReportsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Hospital
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAll();

        return Ok(response);
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Doctor y hospital
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetById(id);

        return Ok(response);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Create([FromBody] PatientRegisterDTO patientDTO)
    {
        var response = await _service.Create(patientDTO);

        return Ok(response);
    }

    //TODO: validar
    [HttpPut("Edit")]
    public async Task<IActionResult> Update([FromBody] PatientDTO patientDTO)
    {
        var response = await _service.Update(patientDTO);

        return Ok(response);
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Hospital
    [HttpDelete("Remove/{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var reponse = await _service.Remove(id);

        return Ok(reponse);
    }

    //TODO: Crear login

}
