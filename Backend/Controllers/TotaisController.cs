using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

/// <summary>
/// Responsável por gerar o relatório de totais e saldos para o Dashboard.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public TotaisController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Calcula e retorna os totais de receitas, despesas e saldos individuais e gerais.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // O SQLite prefere a sintaxe de condição ternária ( ? : ).
        // Isso é convertido de forma extremamente eficiente para um "SUM(CASE WHEN...)" em SQL.
        var relatorioPessoas = await _context.Pessoas
            .AsNoTracking()
            .Select(p => new TotaisPessoaDto
            {
                Nome = p.Nome,
                Receitas = p.Transacoes.Sum(t => t.Tipo == TipoTransacao.Receita ? t.Valor : 0),
                Despesas = p.Transacoes.Sum(t => t.Tipo == TipoTransacao.Despesa ? t.Valor : 0),
                Saldo = p.Transacoes.Sum(t => t.Tipo == TipoTransacao.Receita ? t.Valor : 0) - 
                        p.Transacoes.Sum(t => t.Tipo == TipoTransacao.Despesa ? t.Valor : 0)
            })
            .OrderBy(p => p.Nome)
            .ToListAsync();

        var relatorio = new RelatorioTotaisDto
        {
            Pessoas = relatorioPessoas,
            TotalReceitas = relatorioPessoas.Sum(p => p.Receitas),
            TotalDespesas = relatorioPessoas.Sum(p => p.Despesas),
            SaldoLiquido = relatorioPessoas.Sum(p => p.Receitas) - relatorioPessoas.Sum(p => p.Despesas)
        };

        return Ok(relatorio);
    }
}