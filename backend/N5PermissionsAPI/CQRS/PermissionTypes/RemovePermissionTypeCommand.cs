using MediatR;
using N5PermissionsAPI.Core.Common;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class RemovePermissionTypeCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
