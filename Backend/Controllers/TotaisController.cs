using Backend.Data;
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
    public async Task<IActionResult> GetTotais()
    {
        var pessoas = await _context.Pessoas.Include(p => p.Transacoes).ToListAsync();

        var totaisPessoas = pessoas.Select(p => {
            var totalReceitas = p.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
            var totalDespesas = p.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);
            return new 
            {
                p.Id,
                p.Nome,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            };
        }).ToList();

        var totalGeralReceitas = totaisPessoas.Sum(p => p.TotalReceitas);
        var totalGeralDespesas = totaisPessoas.Sum(p => p.TotalDespesas);
        
        var relatorio = new
        {
            Pessoas = totaisPessoas,
            TotalGeral = new 
            {
                Receitas = totalGeralReceitas,
                Despesas = totalGeralDespesas,
                SaldoLiquido = totalGeralReceitas - totalGeralDespesas
            }
        };

        return Ok(relatorio);
    }
}