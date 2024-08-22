namespace N5PermissionsAPI.Core.DTOs;

public class PermissionDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeLastName { get; set; }
    public int PermissionTypeId { get; set; }
    public DateTime PermissionDate { get; set; }
    public string PermissionTypeName { get; set; }
}