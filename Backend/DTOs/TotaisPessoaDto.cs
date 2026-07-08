namespace Backend.DTOs;

/// <summary>
/// Representa o consolidado financeiro de uma única pessoa.
/// </summary>
public class TotaisPessoaDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal Receitas { get; set; }
    public decimal Despesas { get; set; }
    public decimal Saldo { get; set; }
}