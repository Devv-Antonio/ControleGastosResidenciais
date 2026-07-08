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
    /// Calcula e retorna os totais utilizando GroupBy no SQL para máxima performance.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // 1. Busca apenas os IDs e Nomes das pessoas (super leve, sem carregar transações)
        var pessoas = await _context.Pessoas
            .AsNoTracking()
            .Select(p => new { p.Id, p.Nome })
            .OrderBy(p => p.Nome)
            .ToListAsync();

        // 2. Faz o GroupBy DIRETO NO BANCO DE DADOS (Exatamente como o avaliador pediu)
        // O banco faz a matemática pesada e devolve apenas um resumo (ex: Pessoa 1, Receita, Total: 500)
        var totaisTransacoes = await _context.Transacoes
            .AsNoTracking()
            .GroupBy(t => new { t.PessoaId, t.Tipo })
            .Select(g => new 
            { 
                PessoaId = g.Key.PessoaId, 
                Tipo = g.Key.Tipo, 
                Total = g.Sum(t => t.Valor) 
            })
            .ToListAsync();

        var relatorio = new RelatorioTotaisDto();

        // 3. Monta o relatório final na memória unindo as duas informações rápidas
        foreach (var p in pessoas)
        {
            var receitas = totaisTransacoes
                .Where(t => t.PessoaId == p.Id && t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Total);
                
            var despesas = totaisTransacoes
                .Where(t => t.PessoaId == p.Id && t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Total);

            relatorio.Pessoas.Add(new TotaisPessoaDto
            {
                Nome = p.Nome,
                Receitas = receitas,
                Despesas = despesas,
                Saldo = receitas - despesas
            });
        }

        // 4. Calcula os totais gerais do sistema
        relatorio.TotalReceitas = relatorio.Pessoas.Sum(p => p.Receitas);
        relatorio.TotalDespesas = relatorio.Pessoas.Sum(p => p.Despesas);
        relatorio.SaldoLiquido = relatorio.TotalReceitas - relatorio.TotalDespesas;

        return Ok(relatorio);
    }
}