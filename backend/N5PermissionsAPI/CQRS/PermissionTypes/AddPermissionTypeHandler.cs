using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;
using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class AddPermissionTypeHandler : IRequestHandler<AddPermissionTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPermissionTypeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddPermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var permissionType = new PermissionType
            {
                Description = request.Description
            };

            await _unitOfWork.PermissionTypes.AddAsync(permissionType);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(permissionType.Id);
        }
    }
}
