using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.DTOs;

namespace N5PermissionsAPI.Application.Features.GetPermissions;

public class GetPermissionsQuery : IRequest<Result<PaginatedResult<PermissionDto>>>
{
    public PaginationParams Params { get; set; } = new PaginationParams { PageNumber = 1, PageSize = 10 };
}
