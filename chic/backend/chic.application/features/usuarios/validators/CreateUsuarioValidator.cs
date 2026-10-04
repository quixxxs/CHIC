using FluentValidation;
using chic.application.features.usuarios.DTOs;

namespace chic.application.features.usuarios.validators;

public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioRequestDTO>
{
    public CreateUsuarioValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");
        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.")
            .MinimumLength(3).WithMessage("El apellido debe tener al menos 3 caracteres.");
        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El correo no tiene un formato válido.")
            .MaximumLength(150).WithMessage("El correo no puede superar 150 caracteres.");
        RuleFor(x => x.Telefono)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.");
        RuleFor(x => x.Cargo)
            .MaximumLength(100).WithMessage("El cargo no puede superar 100 caracteres.")
            .MinimumLength(3).WithMessage("El cargo debe tener al menos 3 caracteres.");
        RuleFor(x => x.EmpresaId)
            .GreaterThan(0).WithMessage("Debe indicar una empresa válida.");
    }
}