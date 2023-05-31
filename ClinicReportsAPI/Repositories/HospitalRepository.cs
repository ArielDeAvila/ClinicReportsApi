using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicReportsAPI.Repositories;

public class HospitalRepository : GenericRepository<Hospital>, IHospitalRepository
{
	public HospitalRepository(SystemReportContext context):base(context)
	{
		
	}

    public override async Task<IEnumerable<Hospital>> GetAll()
    {
		var hospitals = await _context.Hospitals.Where(h => h.AuditDateDelete==null)
										  .Include(h => h.MedicalServices)
										  .AsNoTracking().ToListAsync();

        return hospitals;
    }

	public override async Task<Hospital> GetById(int id)
	{
		var hospital = await _context.Hospitals.Include(h => h.MedicalServices)
						   .AsNoTracking().FirstOrDefaultAsync(h => h.AuditDateDelete == null && h.Id.Equals(id));

		return hospital!;								   
	}

	public async Task<Hospital> GetHospital(Expression<Func<Hospital, bool>> expression)
	{
		var hospital = await _context.Hospitals.Include(h => h.MedicalServices)
			.AsNoTracking().FirstOrDefaultAsync(expression);

		return hospital!;
	}

    public async void Update(Hospital hospital, List<MedicalService> medicalServices)
    {
		var existingHospital = await _context.Hospitals.Include(h => h.MedicalServices)
													   .FirstAsync(h => h.Id.Equals(hospital.Id));

		existingHospital.AuditDateUpdate = DateTime.Now;
		existingHospital.Name = hospital.Name;
		existingHospital.Address = hospital.Address;
		existingHospital.PhoneNumber = hospital.PhoneNumber;

		//Revisa dentro de los servicios existentes sí alguno de ellos no se encuentra en la nueva lista
		//de servicios que se entra por parámetro, de ser así lo elimina. 
		foreach (var service in existingHospital.MedicalServices)
			if (!medicalServices.Exists(ms => ms.Name.StartsWith(service.Name))) // si es un servicio nuevo no tendrá Id
				service.AuditDateDelete = DateTime.Now;

		var newMedicalServices = new List<MedicalService>();
		var existingMedicalServices = existingHospital.MedicalServices.ToList();

        //Revisa en la nueva lista de servicios que entra por parámetro sí hay servicios que no se encuentre
        //en la lista de servicios existentes, de ser así los agrega a lista de nuevos servicios
        foreach (var medicalService in medicalServices)
			if(!existingMedicalServices.Exists(hs => hs.Id.Equals(medicalService.Id)))
				newMedicalServices.Add(medicalService);

        _context.Hospitals.Update(existingHospital);
		_context.Entry(existingHospital).Property(h => h.Identification).IsModified = false;
        _context.Entry(existingHospital).Property(h => h.Email).IsModified = false;

        if (newMedicalServices.Count > 0) 
			_context.MedicalServices.AddRange(newMedicalServices);
    }

    public void Create(Hospital hospital, List<MedicalService> medicalServices)
	{
		hospital.AuditDateCreated = DateTime.Now;
		
		foreach(var service in medicalServices) service.HospitalId = hospital.Id;

		//se agregan las entidades al contexto
		_context.Hospitals.Add(hospital);
		_context.MedicalServices.AddRange(medicalServices);

	}

    public void UpdateEmail(Hospital hospital)
    {
        _context.Hospitals.Update(hospital);
        _context.Entry(hospital).Property(d => d.Identification).IsModified = false;
        _context.Entry(hospital).Property(d => d.Password).IsModified = false;
    }

    public void UpdatePassword(Hospital hospital)
    {
        _context.Hospitals.Update(hospital);
        _context.Entry(hospital).Property(d => d.Identification).IsModified = false;
        _context.Entry(hospital).Property(d => d.Email).IsModified = false;
    }


}
