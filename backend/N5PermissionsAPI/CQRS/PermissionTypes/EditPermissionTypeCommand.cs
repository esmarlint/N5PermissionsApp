using MediatR;
using N5PermissionsAPI.Core.Common;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class EditPermissionTypeCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
