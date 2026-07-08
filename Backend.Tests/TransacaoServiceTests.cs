using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests;

/// <summary>
/// Classe de testes automatizados para validar as regras de negócio das transações.
/// </summary>
public class TransacaoServiceTests
{
    // Função auxiliar para criar um banco de dados temporário e isolado para cada teste
    private DbContextOptions<AppDbContext> GetInMemoryOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task CriarAsync_MenorDeIdade_ComReceita_DeveLancarInvalidOperationException()
    {
        // 1. ARRANGE (Preparação do cenário)
        var options = GetInMemoryOptions();
        using var context = new AppDbContext(options);

        // Criamos um usuário menor de idade no banco temporário
        var pessoa = new Pessoa { Nome = "Adolescente Jovem", Idade = 17 };
        context.Pessoas.Add(pessoa);
        await context.SaveChangesAsync();

        var service = new TransacaoService(context);

        // Tentamos lançar uma Receita para esse menor de idade
        var dto = new TransacaoCreateDto
        {
            PessoaId = pessoa.Id,
            Valor = 150.00m,
            Tipo = TipoTransacao.Receita,
            Descricao = "Mesada"
        };

        // 2 & 3. ACT & ASSERT (Ação e Validação)
        // O teste passa se o sistema bloquear a operação e devolver a mensagem exata de erro
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CriarAsync(dto));
        
        Assert.Equal("Pessoas menores de 18 anos só podem registrar despesas.", exception.Message);
    }
}