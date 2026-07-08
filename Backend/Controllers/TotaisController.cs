using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TotaisController : ControllerBase
{
    private readonly AppDbContext _context;

    public TotaisController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // 1. Busca as pessoas ordenadas
        var pessoas = await _context.Pessoas
            .AsNoTracking()
            .OrderBy(p => p.Nome)
            .ToListAsync();

        // 2. Busca as transações
        var transacoes = await _context.Transacoes
            .AsNoTracking()
            .ToListAsync();

        // 3. O C# faz o cálculo de forma super rápida e segura
        var relatorioPessoas = pessoas.Select(p => 
        {
            var receitas = transacoes
                .Where(t => t.PessoaId == p.Id && t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);
                
            var despesas = transacoes
                .Where(t => t.PessoaId == p.Id && t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            return new TotaisPessoaDto
            {
                Nome = p.Nome,
                Receitas = receitas,
                Despesas = despesas,
                Saldo = receitas - despesas
            };
        }).ToList();

        // 4. Monta o objeto final sem risco de listas nulas
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