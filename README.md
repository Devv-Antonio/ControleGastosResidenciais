# Controle de Gastos Residenciais

Sistema web para controle de gastos de uma residência, permitindo cadastrar as pessoas do domicílio, registrar suas transações financeiras (receitas e despesas) e consultar o balanço financeiro individual e geral.

## 🧩 Funcionalidades

- **Cadastro de Pessoas**: criação, listagem e exclusão. Ao excluir uma pessoa, todas as suas transações são removidas automaticamente (exclusão em cascata).
- **Cadastro de Transações**: criação e listagem de receitas e despesas vinculadas a uma pessoa já cadastrada.
  - Regra de negócio: pessoas menores de 18 anos só podem ter **despesas** cadastradas em seu nome.
- **Consulta de Totais**: total de receitas, despesas e saldo por pessoa, além do total geral consolidado de todos os moradores.

## 🛠️ Tecnologias

**Back-end**
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQLite (persistência em arquivo local)
- Swagger / OpenAPI

**Front-end**
- React 19 + TypeScript
- Vite
- React Router
- Axios

## 📁 Estrutura do projeto

```
ControleGastosResidenciais/
├── Backend/
│   ├── Controllers/       # Endpoints da API (Pessoas, Transacoes, Totais)
│   ├── Data/               # DbContext e configuração do EF Core
│   ├── Models/             # Entidades do domínio
│   ├── Migrations/         # Histórico de migrações do banco (gerado pelo EF Core)
│   ├── Program.cs          # Configuração e inicialização da aplicação
│   └── appsettings.json    # Configurações e connection string
└── Frontend/
    ├── src/
    │   ├── pages/           # Telas: Dashboard, Pessoas, Transações
    │   ├── services/        # Cliente HTTP (Axios) para consumo da API
    │   ├── App.tsx           # Rotas e layout principal
    │   └── main.tsx          # Ponto de entrada da aplicação
    └── package.json
```

## ▶️ Como executar

### Pré-requisitos
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)

### Back-end

```bash
cd Backend
dotnet restore
dotnet ef database update   # cria o banco SQLite e aplica as migrações
dotnet run
```

A API sobe por padrão em `http://localhost:5054`. Com o ambiente de desenvolvimento, o Swagger fica disponível em `http://localhost:5054/swagger`.

### Front-end

```bash
cd Frontend
npm install
npm run dev
```

A aplicação sobe por padrão em `http://localhost:5173`.

> Certifique-se de que o back-end está rodando antes de abrir o front-end, já que as telas dependem da API para carregar e salvar dados.

## 🔌 Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/pessoas` | Lista todas as pessoas cadastradas |
| POST | `/api/pessoas` | Cadastra uma nova pessoa (`nome`, `idade`) |
| DELETE | `/api/pessoas/{id}` | Remove uma pessoa e todas as suas transações |
| GET | `/api/transacoes` | Lista todas as transações |
| POST | `/api/transacoes` | Cadastra uma nova transação (`descricao`, `valor`, `tipo`, `pessoaId`) |
| GET | `/api/totais` | Retorna o total de receitas, despesas e saldo por pessoa e o total geral |

## 📐 Regras de negócio

- Toda transação precisa referenciar uma pessoa já existente no cadastro (`pessoaId` válido).
- Pessoas com idade menor que 18 anos só podem registrar transações do tipo **despesa**.
- Ao deletar uma pessoa, todas as transações associadas a ela são excluídas automaticamente (cascade delete no banco).

## 💾 Persistência de dados

Os dados são armazenados em um banco SQLite local (`gastos_residenciais.db`, gerado na primeira execução), garantindo que as informações persistam entre reinicializações da aplicação.

## 🗂️ Modelo de dados

**Pessoa**
| Campo | Tipo | Descrição |
|---|---|---|
| Id | int | Identificador único, gerado automaticamente |
| Nome | string | Nome da pessoa |
| Idade | int | Idade da pessoa |

**Transação**
| Campo | Tipo | Descrição |
|---|---|---|
| Id | int | Identificador único, gerado automaticamente |
| Descricao | string | Descrição da transação |
| Valor | decimal | Valor monetário da transação |
| Tipo | enum (Despesa = 0, Receita = 1) | Tipo da transação |
| PessoaId | int | Identificador da pessoa vinculada à transação |

## 📄 Licença

Projeto desenvolvido para fins de avaliação técnica.