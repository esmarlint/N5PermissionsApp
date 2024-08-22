using N5PermissionsAPI.Core.Interfaces;
using N5PermissionsAPI.Core.Models;
using N5PermissionsAPI.Persistence.DbContexts;


namespace N5PermissionsAPI.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IRepository<Permission> Permissions { get; }
    public IRepository<PermissionType> PermissionTypes { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Permissions = new Repository<Permission>(_context);
        PermissionTypes = new Repository<PermissionType>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}