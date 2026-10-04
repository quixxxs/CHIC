// chic.api/controllers/ubicacionesController.cs
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using chic.application.interfaces;
using chic.domain;
using chic.application.features.ubicaciones.DTOs;

namespace chic.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UbicacionesController : ControllerBase
{
    private readonly IUbicacionRepository _repository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IValidator<CreateUbicacionRequestDTO> _createValidator;
    private readonly IValidator<UpdateUbicacionRequestDto> _updateValidator;

    public UbicacionesController(
        IUbicacionRepository repository,
        IEmpresaRepository empresaRepository,
        IValidator<CreateUbicacionRequestDTO> createValidator,
        IValidator<UpdateUbicacionRequestDto> updateValidator)
    {
        _repository = repository;
        _empresaRepository = empresaRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private static UbicacionResponseDTO ToResponse(Ubicacion u) => new()
    {
        Id = u.Id, Nombre = u.Nombre, Latitud = u.Latitud,
        Longitud = u.Longitud, RadioMetros = u.RadioMetros, EmpresaId = u.EmpresaId
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ubicaciones = await _repository.GetAllAsync();
        return Ok(ubicaciones.Select(ToResponse).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ubicacion = await _repository.GetByIdAsync(id);
        if (ubicacion is null) return NotFound();
        return Ok(ToResponse(ubicacion));
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateUbicacionRequestDTO request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        if (!await _empresaRepository.ExistsAsync(request.EmpresaId))
            return BadRequest(new[] { "La empresa indicada no existe." });

        var creada = await _repository.AddAsync(new Ubicacion
        {
            Nombre = request.Nombre.Trim(),
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            RadioMetros = request.RadioMetros,
            EmpresaId = request.EmpresaId
        });

        var response = ToResponse(creada);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateUbicacionRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var ubicacion = await _repository.GetByIdAsync(id);
        if (ubicacion is null) return NotFound();

        ubicacion.Nombre = request.Nombre.Trim();
        ubicacion.Latitud = request.Latitud;
        ubicacion.Longitud = request.Longitud;
        ubicacion.RadioMetros = request.RadioMetros;
        await _repository.UpdateAsync(ubicacion);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ubicacion = await _repository.GetByIdAsync(id);
        if (ubicacion is null) return NotFound();
        if (await _repository.TieneAsistenciasAsync(id))
            return Conflict(
                "No se puede eliminar la ubicación porque tiene asistencias registradas."
                );

        await _repository.DeleteAsync(ubicacion);
        return NoContent();
    }
}