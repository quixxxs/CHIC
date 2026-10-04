// chic.api/controllers/usuariosController.cs
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using chic.application.interfaces;
using chic.domain;
using chic.application.features.usuarios.DTOs;

namespace chic.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IChicRepository _chicRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IValidator<CreateUsuarioRequestDTO> _createValidator;
    private readonly IValidator<UpdateChicRequestDto> _updateValidator;

    public UsuariosController(
        IChicRepository chicRepository,
        IEmpresaRepository empresaRepository,
        IValidator<CreateUsuarioRequestDTO> createValidator,
        IValidator<UpdateChicRequestDto> updateValidator)
    {
        _chicRepository = chicRepository;
        _empresaRepository = empresaRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private static UsuarioResponseDTO ToResponse(Usuario u) => new()
    {
        Id = u.Id, Nombre = u.Nombre, Apellido = u.Apellido, Correo = u.Correo,
        Telefono = u.Telefono, Cargo = u.Cargo, Activo = u.Activo,
        FechaCreacion = u.FechaCreacion, EmpresaId = u.EmpresaId
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _chicRepository.GetAllAsync();
        return Ok(usuarios.Select(ToResponse).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _chicRepository.GetByIdAsync(id);
        if (usuario is null) return NotFound();
        return Ok(ToResponse(usuario));
    }

    [HttpPost]
    public async Task<IActionResult> CrearUsuario([FromBody] CreateUsuarioRequestDTO request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        if (!await _empresaRepository.ExistsAsync(request.EmpresaId))
            return BadRequest(new[] { "La empresa indicada no existe." });

        var correo = request.Correo.Trim().ToLowerInvariant();
        if (await _chicRepository.ExisteCorreoAsync(correo))
            return Conflict("Ya existe un usuario con ese correo.");   // 409

        var creado = await _chicRepository.AddAsync(new Usuario
        {
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido.Trim(),
            Correo = correo,
            Telefono = request.Telefono?.Trim(),
            Cargo = request.Cargo?.Trim(),
            EmpresaId = request.EmpresaId
        });

        var response = ToResponse(creado);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateChicRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var usuario = await _chicRepository.GetByIdAsync(id);
        if (usuario is null) return NotFound();

        if (!await _empresaRepository.ExistsAsync(request.EmpresaId))
            return BadRequest(new[] { "La empresa indicada no existe." });

        var correo = request.Correo.Trim().ToLowerInvariant();
        if (await _chicRepository.ExisteCorreoAsync(correo, excluirId: id))
            return Conflict("Ya existe otro usuario con ese correo.");

        usuario.Nombre = request.Nombre.Trim();
        usuario.Apellido = request.Apellido.Trim();
        usuario.Correo = correo;
        usuario.Telefono = request.Telefono?.Trim();
        usuario.Cargo = request.Cargo?.Trim();
        usuario.Activo = request.Activo;
        usuario.EmpresaId = request.EmpresaId;
        await _chicRepository.UpdateAsync(usuario);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuario = await _chicRepository.GetByIdAsync(id);
        if (usuario is null) return NotFound();
        if (await _chicRepository.TieneAsistenciasAsync(id))
            return Conflict(
                "No se puede eliminar el usuario porque tiene asistencias registradas."
                );

        await _chicRepository.DeleteAsync(usuario);
        return NoContent();
    }
}