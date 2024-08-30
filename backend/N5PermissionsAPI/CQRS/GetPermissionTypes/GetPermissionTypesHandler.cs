using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;
using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.CQRS.GetPermissionTypes
{
    public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuery, Result<IEnumerable<PermissionType>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPermissionTypesHandler> _logger;

        public GetPermissionTypesHandler(IUnitOfWork unitOfWork, ILogger<GetPermissionTypesHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<PermissionType>>> Handle(GetPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var permissionTypes = await _unitOfWork.PermissionTypes.GetAllAsync();
                return Result<IEnumerable<PermissionType>>.Success(permissionTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permission types");
                return Result<IEnumerable<PermissionType>>.Fail("An error occurred while retrieving permission types.");
            }
        }
    }
}