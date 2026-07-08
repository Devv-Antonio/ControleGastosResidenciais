namespace Backend.DTOs;

/// <summary>
/// Estrutura padronizada para retornos paginados na API.
/// </summary>
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    
    // Calcula automaticamente o total de páginas com base no total de itens
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}