
using FluentValidation;
using N5PermissionsAPI.Application.Features.GetPermissions;

namespace N5PermissionsAPI.CQRS.GetPermissions
{
    public class GetPermissionByIdValidator : AbstractValidator<GetPermissionByIdQuery>
    {
        public GetPermissionByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del permiso debe ser un número positivo.");
        }
    }
}