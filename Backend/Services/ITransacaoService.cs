using Backend.DTOs;
using Backend.Models;

namespace Backend.Services;

/// <summary>
/// Contrato de operações permitidas para as transações.
/// </summary>
public interface ITransacaoService
{
    Task<IEnumerable<Transacao>> ObterTodasAsync();
    Task<Transacao> CriarAsync(TransacaoCreateDto dto);
}