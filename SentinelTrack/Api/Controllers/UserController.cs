using Microsoft.AspNetCore.Mvc;
using SentinelTrack.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using SentinelTrack.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;

namespace SentinelTrack.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[SwaggerTag("Controlador responsável pelo gerenciamento de usuários e suas operações.")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Busca todos os usuários.
    /// </summary>
    /// <param name="page">Número da página (começa em 1)</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <param name="email">Filtro opcional por email</param>
    /// <returns>Lista de usuários</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? email = null)
    {
        var result = await _userService.GetAllAsync(page, pageSize, email);
        return Ok(result);
    }

    /// <summary>
    /// Busca um usuário por id.
    /// </summary>
    /// <param name="id">Id do usuário desejado</param>
    /// <returns>Usuário com o id informado</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="request">Dados do usuário para criação</param>
    /// <returns>Usuário criado</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] UserRequest request)
    {
        var created = await _userService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <param name="id">Id do usuário a ser atualizado</param>
    /// <param name="request">Dados do usuário para atualização</param>
    /// <returns>Confirmação do usuário atualizado</returns>
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserRequest request)
    {
        var updated = await _userService.UpdateAsync(id, request);
        if (!updated) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Deleta um usuário existente
    /// </summary>
    /// <param name="id">Id do usuário a ser deletado</param>
    /// <returns>Confirmação do usuário removido</returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}