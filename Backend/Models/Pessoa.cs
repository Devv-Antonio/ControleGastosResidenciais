using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models;

/// <summary>
/// Representa uma pessoa cadastrada no sistema. 
/// Contém validações para garantir a integridade dos dados no banco.
/// </summary>
public class Pessoa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 130, ErrorMessage = "A idade deve estar entre 0 e 130 anos.")]
    public int Idade { get; set; }

    /// <summary>
    /// Lista de transações da pessoa. 
    /// O JsonIgnore evita loops infinitos ao retornar os dados na API.
    /// </summary>
    [JsonIgnore]
    public List<Transacao> Transacoes { get; set; } = new();
}