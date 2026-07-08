namespace Backend.DTOs;

/// <summary>
/// Relatório completo contendo a lista individualizada e o total geral do sistema.
/// </summary>
public class RelatorioTotaisDto
{
    public List<TotaisPessoaDto> Pessoas { get; set; } = new();
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal SaldoLiquido { get; set; }
}