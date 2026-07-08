using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

/// <summary>
/// Gerencia as operações de cadastro, listagem e deleção de pessoas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly AppDbContext _context;

    public PessoasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista as pessoas cadastradas com paginação e ordenação.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // 1. Prepara a consulta base
        var query = _context.Pessoas.AsNoTracking();

        // 2. Conta o total de registos no banco (antes de paginar)
        var totalCount = await query.CountAsync();

        // 3. Aplica a ordenação, salta os registos anteriores e pega o tamanho da página
        var pessoas = await query
            .OrderBy(p => p.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 4. Monta o objeto de resposta padronizado
        var resultado = new PagedResult<Pessoa>
        {
            Items = pessoas,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
            
        return Ok(resultado);
    }

    /// <summary>
    /// Busca uma pessoa específica pelo seu identificador (ID).
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        
        if (pessoa == null) 
            return NotFound(new { message = "Pessoa não encontrada." });
            
        return Ok(pessoa);
    }

    /// <summary>
    /// Cadastra uma nova pessoa no sistema utilizando os dados validados do DTO.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PessoaCreateDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
    }

    /// <summary>
    /// Deleta uma pessoa e todas as suas transações em cascata.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        
        if (pessoa == null) 
            return NotFound(new { message = "Pessoa não encontrada para deleção." });

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}