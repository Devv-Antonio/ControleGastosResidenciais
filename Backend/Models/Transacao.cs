using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

/// <summary>
/// Representa uma movimentação financeira (Receita ou Despesa).
/// </summary>
public class Transacao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "O ID da pessoa é obrigatório.")]
    public int PessoaId { get; set; }

    /// <summary>
    /// Propriedade de navegação do Entity Framework.
    /// </summary>
    public Pessoa? Pessoa { get; set; }
}