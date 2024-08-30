using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class RemovePermissionTypeHandler : IRequestHandler<RemovePermissionTypeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemovePermissionTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(RemovePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.Id);
            if (permissionType == null)
            {
                return Result<bool>.Fail("Permission type not found");
            }

            _unitOfWork.PermissionTypes.Delete(permissionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
