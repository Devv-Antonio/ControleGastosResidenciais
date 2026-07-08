using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Middlewares;

/// <summary>
/// Interceta todas as exceções da aplicação, regista logs e devolve respostas padronizadas no formato ProblemDetails.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // 1. Regista o erro para auditoria interna
        _logger.LogError(exception, "Exceção capturada: {Message}", exception.Message);

        // 2. Estrutura a resposta no padrão RFC 7807 (ProblemDetails)
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        // 3. Define a resposta com base no tipo de erro
        if (exception is ArgumentException || exception is InvalidOperationException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Erro de Validação de Negócio";
            problemDetails.Detail = exception.Message;
        }
        else
        {
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Erro Interno do Servidor";
            problemDetails.Detail = "Ocorreu um erro inesperado. A nossa equipa já foi notificada.";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}