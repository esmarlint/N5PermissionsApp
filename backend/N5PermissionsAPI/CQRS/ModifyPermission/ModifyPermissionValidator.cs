
using FluentValidation;

namespace N5PermissionsAPI.CQRS.ModifyPermission
{
    public class ModifyPermissionValidator : AbstractValidator<ModifyPermissionCommand>
    {
        public ModifyPermissionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del permiso debe ser un número positivo.");

            RuleFor(x => x.EmployeeName)
                .NotEmpty().WithMessage("El nombre del empleado es requerido.")
                .MaximumLength(100).WithMessage("El nombre del empleado no puede exceder los 100 caracteres.");

            RuleFor(x => x.EmployeeLastName)
                .NotEmpty().WithMessage("El apellido del empleado es requerido.")
                .MaximumLength(100).WithMessage("El apellido del empleado no puede exceder los 100 caracteres.");

            RuleFor(x => x.PermissionTypeId)
                .GreaterThan(0).WithMessage("El tipo de permiso debe ser un número positivo.");
        }
    }
}