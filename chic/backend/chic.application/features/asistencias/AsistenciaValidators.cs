// chic.application/features/asistencias/validators/AsistenciaValidators.cs
using FluentValidation;
using chic.application.features.asistencias.DTOs;
namespace chic.application.features.asistencias.validators;

public class CreateAsistenciaRequestValidator : AbstractValidator<CreateAsistenciaRequestDTO>
{
    public CreateAsistenciaRequestValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("Debe indicar un usuario válido.");
        RuleFor(x => x.Tipo).IsInEnum().WithMessage("El tipo debe ser Entrada o Salida.");
        RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).WithMessage("La latitud debe estar entre -90 y 90.");
        RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).WithMessage("La longitud debe estar entre -180 y 180.");
    }
}

public class UpdateAsistenciaRequestValidator : AbstractValidator<UpdateAsistenciaRequestDto>
{
    public UpdateAsistenciaRequestValidator()
    {
        RuleFor(x => x.Tipo).IsInEnum().WithMessage("El tipo debe ser Entrada o Salida.");
        RuleFor(x => x.Observacion).MaximumLength(250).WithMessage("La observación no puede superar 250 caracteres.");
    }
}