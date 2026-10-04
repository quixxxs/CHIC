// chic.api/controllers/asistenciasController.cs
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using chic.application.interfaces;
using chic.application.services;
using chic.application.features.asistencias.DTOs;
using chic.domain;

namespace chic.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AsistenciasController : ControllerBase
{
    private readonly IAsistenciaRepository _repository;
    private readonly IChicRepository _usuarioRepository;
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IValidator<CreateAsistenciaRequestDTO> _createValidator;
    private readonly IValidator<UpdateAsistenciaRequestDto> _updateValidator;

    public AsistenciasController(
        IAsistenciaRepository repository,
        IChicRepository usuarioRepository,
        IUbicacionRepository ubicacionRepository,
        IValidator<CreateAsistenciaRequestDTO> createValidator,
        IValidator<UpdateAsistenciaRequestDto> updateValidator)
    {
        _repository = repository;
        _usuarioRepository = usuarioRepository;
        _ubicacionRepository = ubicacionRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private static AsistenciaResponseDTO ToResponse(Asistencia a) => new()
    {
        Id = a.Id, UsuarioId = a.UsuarioId, UbicacionId = a.UbicacionId,
        Tipo = a.Tipo, FechaHora = a.FechaHora, Latitud = a.Latitud,
        Longitud = a.Longitud, DistanciaMetros = a.DistanciaMetros,
        Observacion = a.Observacion
    };

    // GET api/asistencias  o  api/asistencias?usuarioId=1
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? usuarioId)
    {
        var asistencias = await _repository.GetAllAsync(usuarioId);
        return Ok(asistencias.Select(ToResponse).ToList());          // 200
    }

    // GET api/asistencias/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var asistencia = await _repository.GetByIdAsync(id);
        if (asistencia is null) return NotFound();                   // 404
        return Ok(ToResponse(asistencia));                           // 200
    }

    // POST api/asistencias: marcar asistencia
    [HttpPost]
    public async Task<IActionResult> Marcar([FromBody] CreateAsistenciaRequestDTO request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));    // 400

        // 1. El usuario debe existir y estar activo
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario is null)
            return BadRequest(new[] { "El usuario indicado no existe." });
        if (!usuario.Activo)
            return BadRequest(new[] { "El usuario está inactivo y no puede marcar asistencia." });

        // 2. La marcación debe alternar: Entrada -> Salida -> Entrada...
        var ultima = await _repository.GetUltimaPorUsuarioAsync(usuario.Id);
        var esperado = (ultima is null || ultima.Tipo == TipoAsistencia.Salida)
            ? TipoAsistencia.Entrada
            : TipoAsistencia.Salida;
        if (request.Tipo != esperado)
            return Conflict($"Se esperaba una marcación de tipo {esperado}.");   // 409

        // 3. La empresa debe tener ubicaciones registradas
        var ubicaciones = (await _ubicacionRepository.GetByEmpresaIdAsync(usuario.EmpresaId)).ToList();
        if (ubicaciones.Count == 0)
            return Conflict("La empresa del usuario no tiene ubicaciones registradas.");

        // 4. Verificar que esté dentro del radio de alguna sede
        var calculadas = ubicaciones.Select(u => new
        {
            Ubicacion = u,
            Distancia = GeoCalculator.DistanciaEnMetros(request.Latitud, request.Longitud, u.Latitud, u.Longitud)
        }).ToList();

        var coincidencia = calculadas
            .Where(x => x.Distancia <= x.Ubicacion.RadioMetros)
            .OrderBy(x => x.Distancia)
            .FirstOrDefault();

        if (coincidencia is null)
        {
            var cercana = calculadas.OrderBy(x => x.Distancia).First();
            return UnprocessableEntity(new[]
            {
                $"Estás a {Math.Round(cercana.Distancia)} m de '{cercana.Ubicacion.Nombre}'; el máximo permitido es {cercana.Ubicacion.RadioMetros} m."
            });                                                                  // 422
        }

        // 5. Guardar la marcación
        var creada = await _repository.AddAsync(new Asistencia
        {
            UsuarioId = usuario.Id,
            UbicacionId = coincidencia.Ubicacion.Id,
            Tipo = request.Tipo,
            FechaHora = DateTime.UtcNow,
            Latitud = request.Latitud,
            Longitud = request.Longitud,
            DistanciaMetros = Math.Round(coincidencia.Distancia, 2)
        });

        var response = ToResponse(creada);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);   // 201
    }

    // PUT api/asistencias/{id}: corrección administrativa (solo Tipo y Observacion)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateAsistenciaRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var asistencia = await _repository.GetByIdAsync(id);
        if (asistencia is null) return NotFound();

        asistencia.Tipo = request.Tipo;
        asistencia.Observacion = request.Observacion?.Trim();
        await _repository.UpdateAsync(asistencia);

        return NoContent();                                          // 204
    }

    // DELETE api/asistencias/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var asistencia = await _repository.GetByIdAsync(id);
        if (asistencia is null) return NotFound();

        await _repository.DeleteAsync(asistencia);
        return NoContent();                                          // 204
    }
}