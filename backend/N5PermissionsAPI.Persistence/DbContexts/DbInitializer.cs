using Microsoft.EntityFrameworkCore;
using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.Persistence.DbContexts;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.Migrate();
        context.Database.EnsureCreated();
    }
}
