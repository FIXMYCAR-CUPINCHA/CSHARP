using Microsoft.AspNetCore.Mvc;
using SentinelTrack.Application.DTOs.Request;
using SentinelTrack.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SentinelTrack.Api.Controllers;

[ApiController]
[Route("api/v1/motos")]
[SwaggerTag("Controlador responsável pelo gerenciamento de motos e suas operações.")]
public class MotoController : ControllerBase
{
    private readonly IMotoService _motoService;

    public MotoController(IMotoService motoService)
    {
        _motoService = motoService;
    }

    /// <summary>
    /// Busca todas as motos.
    /// </summary>
    /// <param name="page">Número da página (começa em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <param name="plate">Filtro opcional por placa</param>
    /// <param name="yardId">Filtro opcional por id do pátio</param>
    /// <returns>Lista de motos</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? plate = null,
        [FromQuery] Guid? yardId = null)
    {
        var result = await _motoService.GetAllAsync(page, pageSize, plate, yardId);
        return Ok(result);
    }

    /// <summary>
    /// Busca uma moto por id.
    /// </summary>
    /// <param name="id">Id da moto desejada</param>
    /// <returns>Moto com o id informado</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var moto = await _motoService.GetByIdAsync(id);
        if (moto == null) return NotFound();
        return Ok(moto);
    }

    /// <summary>
    /// Cria uma nova moto
    /// </summary>
    /// <param name="request">Dados da moto para criação</param>
    /// <returns>Moto criado</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MotoRequest request)
    {
        var created = await _motoService.CreateAsync(request);
        if (created == null)
            return BadRequest(new { error = "YardId inválido" });

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza uma moto existente
    /// </summary>
    /// <param name="id">Id da moto a ser atualizada</param>
    /// <param name="request">Dados da moto para atualização</param>
    /// <returns>Confirmação da moto atualizada</returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MotoRequest request)
    {
        var updated = await _motoService.UpdateAsync(id, request);
        if (updated == "not_found") return NotFound();
        if (updated == "invalid_yard") return BadRequest(new { error = "YardId inválido" });
        return NoContent();
    }

    /// <summary>
    /// Deleta uma moto existente
    /// </summary>
    /// <param name="id">Id da moto a ser deletada</param>
    /// <returns>Confirmação da moto removida</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _motoService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}