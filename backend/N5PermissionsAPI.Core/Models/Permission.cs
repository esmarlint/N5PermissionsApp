namespace N5PermissionsAPI.Core.Models;

public class Permission
{
    public int Id { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeLastName { get; set; }
    public int PermissionTypeId { get; set; }
    public DateTime PermissionDate { get; set; }

    public virtual PermissionType PermissionType { get; set; }
}
