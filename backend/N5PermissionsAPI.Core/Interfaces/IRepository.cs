namespace N5PermissionsAPI.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    IQueryable<T> GetAllAsQueryable();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
