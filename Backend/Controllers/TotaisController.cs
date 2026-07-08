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
        // O Include carrega as transações atreladas a cada pessoa direto do banco
        var pessoas = await _context.Pessoas.Include(p => p.Transacoes).ToListAsync();

        var relatorio = new RelatorioTotaisDto();

        foreach (var pessoa in pessoas)
        {
            var receitas = pessoa.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
            var despesas = pessoa.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);

            relatorio.Pessoas.Add(new TotaisPessoaDto
            {
                Nome = pessoa.Nome,
                Receitas = receitas,
                Despesas = despesas,
                Saldo = receitas - despesas
            });
        }

        relatorio.TotalReceitas = relatorio.Pessoas.Sum(p => p.Receitas);
        relatorio.TotalDespesas = relatorio.Pessoas.Sum(p => p.Despesas);
        relatorio.SaldoLiquido = relatorio.TotalReceitas - relatorio.TotalDespesas;

        return Ok(relatorio);
    }
}