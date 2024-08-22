using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.Features.GetPermissionTypes
{
    public class GetPermissionTypesQuery : IRequest<Result<IEnumerable<PermissionType>>>
    {
    }
}