using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.DTOs;

namespace N5PermissionsAPI.Application.Features.GetPermissions;

public class GetPermissionByIdQuery : IRequest<Result<PermissionDto>>
{
    public int Id { get; set; }
}
