using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class AddPermissionTypeCommand : IRequest<Result<int>>
    {
        public string Description { get; set; }
    }

    public class RemovePermissionTypeCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }

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

    public class EditPermissionTypeCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

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
