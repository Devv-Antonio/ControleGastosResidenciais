using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs;

/// <summary>
/// Objeto utilizado para receber os dados de lançamento de uma nova transação.
/// </summary>
public class TransacaoCreateDto
{
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200)]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "O identificador da pessoa é obrigatório.")]
    public int PessoaId { get; set; }
}