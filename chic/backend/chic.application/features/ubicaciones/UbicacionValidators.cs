// chic.application/features/ubicaciones/validators/UbicacionValidators.cs
using FluentValidation;
using chic.application.features.ubicaciones.DTOs;
namespace chic.application.features.ubicaciones.validators;

public class CreateUbicacionRequestValidator : AbstractValidator<CreateUbicacionRequestDTO>
{
    public CreateUbicacionRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");
        RuleFor(x => x.RadioMetros).InclusiveBetween(10, 5000).WithMessage("El radio debe estar entre 10 y 5000 metros.");
        RuleFor(x => x.EmpresaId).GreaterThan(0).WithMessage("Debe indicar una empresa válida.");
    }
}

public class UpdateUbicacionRequestValidator : AbstractValidator<UpdateUbicacionRequestDto>
{
    public UpdateUbicacionRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");
        RuleFor(x => x.RadioMetros).InclusiveBetween(10, 5000).WithMessage("El radio debe estar entre 10 y 5000 metros.");
    }
}