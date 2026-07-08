using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

/// <summary>
/// Implementa as regras de negócio e persistência das transações.
/// </summary>
public class TransacaoService : ITransacaoService
{
    private readonly AppDbContext _context;

    public TransacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transacao>> ObterTodasAsync()
    {
        // Traz as transações mais recentes primeiro e sem tracking de memória
        return await _context.Transacoes
            .AsNoTracking()
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }

    public async Task<Transacao> CriarAsync(TransacaoCreateDto dto)
    {
        // Regra 1: Evita criar transações para pessoas inexistentes
        var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
        if (pessoa == null)
        {
            throw new ArgumentException("A pessoa informada não existe no cadastro.");
        }

        // Regra 2: Mantém a regra de negócio que restringe menores de 18 anos a apenas despesas
        if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
        {
            throw new InvalidOperationException("Pessoas menores de 18 anos só podem registrar despesas.");
        }

        var transacao = new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return transacao;
    }
}