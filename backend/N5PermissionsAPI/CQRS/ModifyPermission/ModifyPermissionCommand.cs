using MediatR;
using N5PermissionsAPI.Core.Common;

namespace N5PermissionsAPI.CQRS.ModifyPermission;

public class ModifyPermissionCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeeLastName { get; set; }
    public int PermissionTypeId { get; set; }
}
