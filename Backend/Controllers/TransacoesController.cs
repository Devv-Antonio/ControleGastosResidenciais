using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

/// <summary>
/// Gerencia as operações de criação e listagem de transações.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    // Injeção de dependência do serviço
    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    /// <summary>
    /// Lista todas as transações cadastradas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var transacoes = await _transacaoService.ObterTodasAsync();
        return Ok(transacoes);
    }

    /// <summary>
    /// Registra uma nova transação aplicando as regras de negócio.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TransacaoCreateDto dto)
    {
        try
        {
            var transacao = await _transacaoService.CriarAsync(dto);
            
            // Retorna 201 Created informando que o recurso foi criado com sucesso
            return Created("", transacao); 
        }
        catch (ArgumentException ex)
        {
            // Captura o erro de "Pessoa não existe"
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Captura o erro de "Menor de idade com receita"
            return BadRequest(new { message = ex.Message });
        }
    }
}