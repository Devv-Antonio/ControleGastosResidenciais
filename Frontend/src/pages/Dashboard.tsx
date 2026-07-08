import { useEffect, useState } from 'react';
import { api } from '../services/api';

interface Relatorio {
  pessoas: {
    id: number;
    nome: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
  }[];
  totalGeral: {
    receitas: number;
    despesas: number;
    saldoLiquido: number;
  };
}

export default function Dashboard() {
  const [relatorio, setRelatorio] = useState<Relatorio | null>(null);

  useEffect(() => {
    carregarTotais();
  }, []);

  async function carregarTotais() {
    const response = await api.get('/totais');
    setRelatorio(response.data);
  }

  if (!relatorio) return <p>Carregando totais...</p>;

  return (
    <div>
      <h2>Consulta de Totais</h2>

      <h3>Totais por Pessoa</h3>
      <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%', marginBottom: '30px' }}>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Total Receitas</th>
            <th>Total Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {relatorio.pessoas.map(p => (
            <tr key={p.id}>
              <td>{p.nome}</td>
              <td style={{ color: 'green' }}>R$ {p.totalReceitas.toFixed(2)}</td>
              <td style={{ color: 'red' }}>R$ {p.totalDespesas.toFixed(2)}</td>
              <td style={{ fontWeight: 'bold' }}>R$ {p.saldo.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h3>Total Geral</h3>
      <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%' }}>
        <tbody>
          <tr>
            <td><strong>Receitas Totais:</strong></td>
            <td style={{ color: 'green' }}>R$ {relatorio.totalGeral.receitas.toFixed(2)}</td>
          </tr>
          <tr>
            <td><strong>Despesas Totais:</strong></td>
            <td style={{ color: 'red' }}>R$ {relatorio.totalGeral.despesas.toFixed(2)}</td>
          </tr>
          <tr>
            <td><strong>Saldo Líquido:</strong></td>
            <td style={{ fontWeight: 'bold' }}>R$ {relatorio.totalGeral.saldoLiquido.toFixed(2)}</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}