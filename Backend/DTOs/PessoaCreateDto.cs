using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

/// <summary>
/// Objeto utilizado exclusivamente para receber os dados de criação de uma pessoa.
/// Protege a entidade principal contra vulnerabilidades de Overposting.
/// </summary>
public class PessoaCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 130, ErrorMessage = "A idade deve estar entre 0 e 130 anos.")]
    public int Idade { get; set; }
}