import { useEffect, useState } from 'react';
import { api } from '../services/api';

// Tipagens baseadas no DTO profissional que criamos no Back-end
interface TotaisPessoa {
  nome: string;
  receitas: number;
  despesas: number;
  saldo: number;
}

interface Relatorio {
  pessoas: TotaisPessoa[];
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
}

export default function Dashboard() {
  const [relatorio, setRelatorio] = useState<Relatorio | null>(null);
  const [erro, setErro] = useState('');

  useEffect(() => {
    carregarTotais();
  }, []);

  async function carregarTotais() {
    try {
      const response = await api.get('/totais');
      setRelatorio(response.data);
    } catch (error) {
      setErro('Erro ao carregar o dashboard de totais.');
    }
  }

  // Função utilitária para deixar os números com formato de R$
  function formatarMoeda(valor: number) {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);
  }

  // Tela de loading elegante enquanto o back-end processa
  if (!relatorio && !erro) {
    return (
      <div className="container" style={{ textAlign: 'center', marginTop: '3rem' }}>
        <h2 style={{ color: 'var(--text-muted)' }}>Carregando métricas financeiras...</h2>
      </div>
    );
  }

  return (
    <div className="container">
      <h1 className="title">Dashboard Financeiro</h1>
      
      {erro && <div className="alert-error">{erro}</div>}

      {relatorio && (
        <>
          {/* Painéis Superiores (Cards de Resumo Geral) */}
          <div style={{ display: 'flex', gap: '1rem', marginBottom: '2rem', flexWrap: 'wrap' }}>
            
            <div className="card" style={{ flex: 1, marginBottom: 0, borderTop: '4px solid var(--success-color)' }}>
              <h3 style={{ fontSize: '1rem', color: 'var(--text-muted)', marginBottom: '0.5rem' }}>Total de Receitas</h3>
              <p style={{ fontSize: '1.75rem', fontWeight: 'bold', color: 'var(--success-color)' }}>
                {formatarMoeda(relatorio.totalReceitas)}
              </p>
            </div>
            
            <div className="card" style={{ flex: 1, marginBottom: 0, borderTop: '4px solid var(--danger-color)' }}>
              <h3 style={{ fontSize: '1rem', color: 'var(--text-muted)', marginBottom: '0.5rem' }}>Total de Despesas</h3>
              <p style={{ fontSize: '1.75rem', fontWeight: 'bold', color: 'var(--danger-color)' }}>
                {formatarMoeda(relatorio.totalDespesas)}
              </p>
            </div>
            
            <div className="card" style={{ flex: 1, marginBottom: 0, borderTop: '4px solid var(--primary-color)' }}>
              <h3 style={{ fontSize: '1rem', color: 'var(--text-muted)', marginBottom: '0.5rem' }}>Saldo Líquido Geral</h3>
              <p style={{ fontSize: '1.75rem', fontWeight: 'bold', color: 'var(--text-main)' }}>
                {formatarMoeda(relatorio.saldoLiquido)}
              </p>
            </div>

          </div>

          {/* Tabela de Detalhamento por Pessoa */}
          <div className="card">
            <h2 style={{ fontSize: '1.25rem', marginBottom: '1rem', color: 'var(--text-main)' }}>
              Detalhamento por Pessoa
            </h2>
            <div className="table-container">
              <table>
                <thead>
                  <tr>
                    <th>Nome</th>
                    <th>Receitas</th>
                    <th>Despesas</th>
                    <th>Saldo Individual</th>
                  </tr>
                </thead>
                <tbody>
                  {relatorio.pessoas.map((pessoa, index) => (
                    <tr key={index}>
                      <td style={{ fontWeight: 500 }}>{pessoa.nome}</td>
                      <td style={{ color: 'var(--success-color)' }}>{formatarMoeda(pessoa.receitas)}</td>
                      <td style={{ color: 'var(--danger-color)' }}>{formatarMoeda(pessoa.despesas)}</td>
                      <td style={{ 
                        fontWeight: 'bold', 
                        color: pessoa.saldo >= 0 ? 'var(--success-color)' : 'var(--danger-color)' 
                      }}>
                        {formatarMoeda(pessoa.saldo)}
                      </td>
                    </tr>
                  ))}
                  {relatorio.pessoas.length === 0 && (
                    <tr>
                      <td colSpan={4} style={{ textAlign: 'center', color: 'var(--text-muted)' }}>
                        Nenhuma movimentação para exibir.
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </>
      )}
    </div>
  );
}