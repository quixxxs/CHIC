// chic.application/features/empresas/DTOs/EmpresaDTOs.cs
using FluentValidation;
using chic.application.features.empresas.DTOs;
namespace chic.application.features.empresas.validators;

public class CreateEmpresaRequestValidator : AbstractValidator<CreateEmpresaRequestDTO>
{
    public CreateEmpresaRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar 150 caracteres.");
        RuleFor(x => x.Direccion).NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(250).WithMessage("La dirección no puede superar 250 caracteres.");
        RuleFor(x => x.Telefono).MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.");
    }
}

public class UpdateEmpresaRequestValidator : AbstractValidator<UpdateEmpresaRequestDto>
{
    public UpdateEmpresaRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar 150 caracteres.");
        RuleFor(x => x.Direccion).NotEmpty().WithMessage("La dirección es obligatoria.")
            .MaximumLength(250).WithMessage("La dirección no puede superar 250 caracteres.");
        RuleFor(x => x.Telefono).MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.");
    }
}