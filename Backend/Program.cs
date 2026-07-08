using Backend.Data;
using Backend.Middlewares;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. RESILIÊNCIA, LOGS E EXCEÇÕES GLOBAIS ---
builder.Services.AddLogging();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// --- 2. CONTROLADORES E VALIDAÇÕES ---
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Garante que erros de Data Annotations usem o padrão ProblemDetails
        options.SuppressMapClientErrors = false;
    });

// --- 3. BANCO DE DADOS E INJEÇÃO DE DEPENDÊNCIAS ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// --- 4. SWAGGER E SEGURANÇA (CORS) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    // Boa prática: em vez de AllowAnyOrigin, restringe o acesso apenas à porta do Vite
    options.AddPolicy("AllowViteFrontEnd", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ativa o intercetor global de erros
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowViteFrontEnd");
app.UseAuthorization();
app.MapControllers();

app.Run();