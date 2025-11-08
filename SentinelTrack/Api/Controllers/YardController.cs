using Microsoft.AspNetCore.Mvc;
using SentinelTrack.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using SentinelTrack.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace SentinelTrack.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/yards")]
[SwaggerTag("Controlador responsável pelo gerenciamento de pátios e suas operações.")]
public class YardController : ControllerBase
{
    private readonly IYardService _yardService;

    public YardController(IYardService yardService)
    {
        _yardService = yardService;
    }

    /// <summary>
    /// Busca todos os pátios.
    /// </summary>
    /// <param name="page">Número da página (começa em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <returns>Lista de pátios</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _yardService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Busca um pátio por id.
    /// </summary>
    /// <param name="id">Id do pátio desejado</param>
    /// <returns>Pátio com o id informado</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var yard = await _yardService.GetByIdAsync(id);
        if (yard == null) return NotFound();
        return Ok(yard);
    }

    /// <summary>
    /// Cria um novo pátio
    /// </summary>
    /// <param name="request">Dados do pátio para criação</param>
    /// <returns>Pátio criado</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] YardRequest request)
    {
        var created = await _yardService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza um pátio existente
    /// </summary>
    /// <param name="id">Id do pátio a ser atualizado</param>
    /// <param name="request">Dados do pátio para atualização</param>
    /// <returns>Confirmação do pátio atualizado</returns>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] YardRequest request)
    {
        var updated = await _yardService.UpdateAsync(id, request);
        if (!updated) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Deleta um pátio existente
    /// </summary>
    /// <param name="id">Id do pátio a ser deletado</param>
    /// <returns>Confirmação do pátio removido</returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _yardService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}