using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class EditPermissionTypeHandler : IRequestHandler<EditPermissionTypeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditPermissionTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(EditPermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.Id);
            if (permissionType == null)
            {
                return Result<bool>.Fail("Permission type not found");
            }

            permissionType.Description = request.Description;
            _unitOfWork.PermissionTypes.Update(permissionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
