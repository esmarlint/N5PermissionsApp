using Microsoft.EntityFrameworkCore;
using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.Persistence.DbContexts;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        context.Database.ExecuteSqlRaw(
           @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PermissionTypes' and xtype='U')
              CREATE TABLE PermissionTypes (
                  Id INT PRIMARY KEY IDENTITY,
                  Description NVARCHAR(255) NOT NULL
              );");

        context.Database.ExecuteSqlRaw(
            @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Permissions' and xtype='U')
              CREATE TABLE Permissions (
                  Id INT PRIMARY KEY IDENTITY,
                  EmployeeName NVARCHAR(255) NOT NULL,
                  EmployeeLastName NVARCHAR(255) NOT NULL,
                  PermissionTypeId INT NOT NULL,
                  PermissionDate DATETIME2 NOT NULL,
                  FOREIGN KEY (PermissionTypeId) REFERENCES PermissionTypes(Id)
              );");

        if (context.PermissionTypes.Any())
        {
            return;
        }

        var permissionTypes = new PermissionType[]
        {
            new PermissionType { Description = "Lectura" },
            new PermissionType { Description = "Escritura" },
            new PermissionType { Description = "Eliminación" },
            new PermissionType { Description = "Administración" }
        };

        context.PermissionTypes.AddRange(permissionTypes);
        context.SaveChanges();
    }
}
