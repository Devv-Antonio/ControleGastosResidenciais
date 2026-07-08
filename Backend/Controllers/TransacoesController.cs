using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransacoesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var transacoes = await _context.Transacoes.Include(t => t.Pessoa).ToListAsync();
        return Ok(transacoes);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Transacao transacao)
    {
        // Regra: Verifica se a pessoa informada realmente existe
        var pessoa = await _context.Pessoas.FindAsync(transacao.PessoaId);
        if (pessoa == null)
        {
            return BadRequest("Pessoa não encontrada no sistema.");
        }

        // Regra: Menores de idade (menor de 18 anos) apenas despesas podem ser cadastradas
        if (pessoa.Idade < 18 && transacao.Tipo == TipoTransacao.Receita)
        {
            return BadRequest("Menores de 18 anos só podem cadastrar despesas.");
        }

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        
        return Ok(transacao);
    }
}