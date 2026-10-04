// chic.api/controllers/empresasController.cs
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using chic.application.interfaces;
using chic.domain;
using chic.application.features.empresas.DTOs;

namespace chic.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaRepository _repository;
    private readonly IValidator<CreateEmpresaRequestDTO> _createValidator;
    private readonly IValidator<UpdateEmpresaRequestDto> _updateValidator;

    public EmpresasController(
        IEmpresaRepository repository,
        IValidator<CreateEmpresaRequestDTO> createValidator,
        IValidator<UpdateEmpresaRequestDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    private static EmpresaResponseDTO ToResponse(Empresa e) => new()
    {
        Id = e.Id, Nombre = e.Nombre, Direccion = e.Direccion,
        Telefono = e.Telefono, Activa = e.Activa, FechaCreacion = e.FechaCreacion
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var empresas = await _repository.GetAllAsync();
        return Ok(empresas.Select(ToResponse).ToList()); // 200
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var empresa = await _repository.GetByIdAsync(id);
        if (empresa is null) return NotFound(); // 404
        return Ok(ToResponse(empresa)); // 200
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateEmpresaRequestDTO request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));   // 400

        var creada = await _repository.AddAsync(new Empresa
        {
            Nombre = request.Nombre.Trim(),
            Direccion = request.Direccion.Trim(),
            Telefono = request.Telefono?.Trim()
        });

        var response = ToResponse(creada);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);   // 201
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateEmpresaRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var empresa = await _repository.GetByIdAsync(id);
        if (empresa is null) return NotFound();

        empresa.Nombre = request.Nombre.Trim();
        empresa.Direccion = request.Direccion.Trim();
        empresa.Telefono = request.Telefono?.Trim();
        empresa.Activa = request.Activa;
        await _repository.UpdateAsync(empresa);

        return NoContent(); // 204
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var empresa = await _repository.GetByIdAsync(id);
        if (empresa is null) return NotFound();

        if (await _repository.TieneDependenciasAsync(id))
            return Conflict("No se puede eliminar la empresa porque tiene usuarios o ubicaciones asociadas.");   // 409

        await _repository.DeleteAsync(empresa);
        return NoContent(); // 204
    }
}