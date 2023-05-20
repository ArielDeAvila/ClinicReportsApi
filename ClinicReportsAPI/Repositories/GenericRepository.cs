using ClinicReportsAPI.Data;
using ClinicReportsAPI.Data.Entities;
using ClinicReportsAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicReportsAPI.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly SystemReportContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(SystemReportContext context)
    {
        _context = context;
        _entity = _context.Set<T>();
    }


    public virtual async Task<T> GetById(int id)
    {
        var entity = await _entity.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.AuditDateDelete == null);

        return entity!;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        var entities = await _entity.Where(e => e.AuditDateDelete == null).AsNoTracking().ToListAsync();

        return entities;
    }

    public virtual async void Create(T entity)
    {
        entity.AuditDateCreated = DateTime.Now;

        await _entity.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        entity.AuditDateUpdate = DateTime.Now;

        _entity.Update(entity);
    }

    public virtual async void Remove(int id)
    {
        var entity = await GetById(id);

        entity.AuditDateDelete = DateTime.Now;

        _entity.Update(entity);
    }
    
}
