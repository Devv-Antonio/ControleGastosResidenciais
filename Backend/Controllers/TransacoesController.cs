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
        // O tratamento de exceções agora é feito globalmente pelo IExceptionHandler
        var transacao = await _transacaoService.CriarAsync(dto);
        
        return Created("", transacao); 
    }
}