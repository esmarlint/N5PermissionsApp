using MediatR;
using N5PermissionsAPI.Core.Common;

namespace N5PermissionsAPI.Features.RequestPermission
{
    public class RequestPermissionCommand : IRequest<Result<int>>
    {
        public string EmployeeName { get; set; }
        public string EmployeeLastName { get; set; }
        public int PermissionTypeId { get; set; }
    }
}
