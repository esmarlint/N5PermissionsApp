using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.Core.Interfaces;

public interface IUnitOfWork
{
    IRepository<Permission> Permissions { get; }
    IRepository<PermissionType> PermissionTypes { get; }
    Task<int> SaveChangesAsync();
}