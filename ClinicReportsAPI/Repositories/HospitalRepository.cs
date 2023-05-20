using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicReportsAPI.Repositories;

public class HospitalRepository : GenericRepository<Hospital>, IHospitalRepository
{
	public HospitalRepository(SystemReportContext context):base(context)
	{
		
	}

    public override async Task<IEnumerable<Hospital>> GetAll()
    {
		var hospitals = await _context.Hospitals.Where(h => h.AuditDateDelete==null)
										  .Include(h => h.HospitalServices)
										  .AsNoTracking().ToListAsync();

        return hospitals;
    }

	public override async Task<Hospital> GetById(int id)
	{
		var hospital = await _context.Hospitals.Include(h => h.HospitalServices).ThenInclude(h => h.Service)
						   .AsNoTracking().FirstOrDefaultAsync(h => h.AuditDateDelete == null && h.Id == id);

		return hospital!;								   
	}

    public async void Update(Hospital hospital, List<MedicalService> medicalServices)
    {
		var existingHospital = await _context.Hospitals.Include(h => h.HospitalServices).ThenInclude(h => h.Service)
													   .FirstAsync(h => h.Id.Equals(hospital.Id));

		existingHospital.AuditDateUpdate = DateTime.Now;
		existingHospital.Name = hospital.Name;
		existingHospital.Address = hospital.Address;
		existingHospital.Email = hospital.Email;
		existingHospital.PhoneNumber = hospital.PhoneNumber;

		foreach (var service in existingHospital.HospitalServices)
			if (!medicalServices.Exists(ms => ms.Name.StartsWith(service.Service.Name)))
				service.Service.AuditDateDelete = DateTime.Now;

		var newMedicalServices = new List<MedicalService>();

        foreach (var medicalService in medicalServices)
        {
			if(!existingHospital.HospitalServices.ToList().Exists(hs => hs.ServiceId.Equals(medicalService.Id)))
			{
				newMedicalServices.Add(medicalService);
			}
        }

		existingHospital.HospitalServices = HospitalMedicalServices(hospital, medicalServices);

        _context.Hospitals.Update(existingHospital);
		_context.Entry(existingHospital).Property(h => h.Identification).IsModified = false;
		if (newMedicalServices.Count > 0) _context.MedicalServices.AddRange(newMedicalServices);
    }

    public void Create(Hospital hospital, List<MedicalService> medicalServices)
	{
		hospital.AuditDateCreated = DateTime.Now;

		//se asigna la lista creada a la propiedad de navegación del hospital
		hospital.HospitalServices = HospitalMedicalServices(hospital,medicalServices);

		//se agregan las entidades al contexto
		_context.Hospitals.Add(hospital);
		_context.MedicalServices.AddRange(medicalServices);

	}

	private static List<HospitalMedicalService> HospitalMedicalServices(Hospital hospital, List<MedicalService> medicalServices)
	{
        //Se crea una lista de HospitalMedicalService
        var hospitalMedicalServices = new List<HospitalMedicalService>();

        foreach (var medicalService in medicalServices)
        {
            //se crea una nueva instancia de HospitalMedicalService y en sus propiedades de navegación
            //se asignan las instancias correspondientes
            var hospitalMedicalService = new HospitalMedicalService()
            {
                Hospital = hospital,
                Service = medicalService
            };

            //la nueva instancia se agrega a la lista creada
            hospitalMedicalServices.Add(hospitalMedicalService);

            //en la propiedad de navegación del servicio medico se asigna una nueva lista con la nueva instancia
            medicalService.HospitalServices = new List<HospitalMedicalService> { hospitalMedicalService };

        }

		return hospitalMedicalServices;
    }

    //TODO: crear nuevos métodos para el cambio de contraseña y de email

}
