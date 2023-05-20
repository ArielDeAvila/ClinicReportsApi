using ClinicReportsAPI.DTOs;
using ClinicReportsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicReportsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _service;

    public ReportController(IReportService service)
    {
        _service = service;
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo hospital
    [HttpGet("GetAll/Hospital/{id:int}")]
    public async Task<IActionResult> GetAllReportsByHospital(int id)
    {
        var response = await _service.GetAllReportsByHospital(id);

        return Ok(response);
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Doctor
    [HttpGet("GetAll/Doctor/{id:int}")]
    public async Task<IActionResult> GetAllReportsByDoctor(int id)
    {
        var response = await _service.GetAllReportsByDoctor(id);    

        return Ok(response);
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Patient
    [HttpGet("GetAll/Patient/{id:int}")]
    public async Task<IActionResult> GetAllReportsByPatient(int id)
    {
        var response = await _service.GetAllReportsByPatient(id);   

        return Ok(response);    
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetById(id);

        return Ok(response);
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Doctor
    [HttpPost("Save")]
    public async Task<IActionResult> CreateReport([FromBody] ReportDTO reportDTO)
    {
        var response = await _service.Create(reportDTO);

        return Ok(response);
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Doctor y hospital
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateReport([FromBody] ReportDTO reportDTO)
    {
        var response = await _service.Update(reportDTO);

        return Ok(response);    
    }

    //TODO: no deben ser accedida por usuarios que no sean de tipo Doctor y hospital
    [HttpDelete("Remove/{id:int}")]
    public async Task<IActionResult> RemoveReport(int id)
    {
        var response = await _service.Remove(id);

        return Ok(response);
    }
}
