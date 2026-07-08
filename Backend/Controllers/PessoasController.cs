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
    /// Lista todas as pessoas cadastradas de forma ordenada.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // AsNoTracking evita alocação desnecessária de memória.
        // OrderBy garante uma experiência de UI previsível e organizada.
        var pessoas = await _context.Pessoas
            .AsNoTracking()
            .OrderBy(p => p.Nome)
            .ToListAsync();
            
        return Ok(pessoas);
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