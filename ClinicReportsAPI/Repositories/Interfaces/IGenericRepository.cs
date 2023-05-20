using ClinicReportsAPI.Data.Entities;

namespace ClinicReportsAPI.Repositories.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Remove(int id);
}
