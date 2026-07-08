import { useEffect, useState } from 'react';
import type { FormEvent } from 'react';
import { api } from '../services/api';

interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: number;
  pessoa: { nome: string };
}

interface Pessoa {
  id: number;
  nome: string;
}

export default function Transacoes() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState('0'); 
  const [pessoaId, setPessoaId] = useState('');

  useEffect(() => {
    carregarDados();
  }, []);

  async function carregarDados() {
    const resTransacoes = await api.get('/transacoes');
    setTransacoes(resTransacoes.data);

    const resPessoas = await api.get('/pessoas');
    setPessoas(resPessoas.data);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    try {
      await api.post('/transacoes', { 
        descricao, 
        valor: Number(valor), 
        tipo: Number(tipo), 
        pessoaId: Number(pessoaId) 
      });
      setDescricao('');
      setValor('');
      carregarDados();
    } catch (error: any) {
      alert(error.response?.data || 'Erro ao cadastrar transação.');
    }
  }

  return (
    <div>
      <h2>Cadastro de Transações</h2>
      
      <form onSubmit={handleSubmit} style={{ marginBottom: '20px', display: 'flex', gap: '10px' }}>
        <input type="text" placeholder="Descrição" value={descricao} onChange={e => setDescricao(e.target.value)} required />
        <input type="number" step="0.01" placeholder="Valor" value={valor} onChange={e => setValor(e.target.value)} required />
        <select value={tipo} onChange={e => setTipo(e.target.value)}>
          <option value="0">Despesa</option>
          <option value="1">Receita</option>
        </select>
        <select value={pessoaId} onChange={e => setPessoaId(e.target.value)} required>
          <option value="">Selecione a Pessoa</option>
          {pessoas.map(p => (
            <option key={p.id} value={p.id}>{p.nome}</option>
          ))}
        </select>
        <button type="submit">Cadastrar</button>
      </form>

      <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%' }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Descrição</th>
            <th>Valor</th>
            <th>Tipo</th>
            <th>Pessoa</th>
          </tr>
        </thead>
        <tbody>
          {transacoes.map(t => (
            <tr key={t.id}>
              <td>{t.id}</td>
              <td>{t.descricao}</td>
              <td>R$ {t.valor.toFixed(2)}</td>
              <td>{t.tipo === 0 ? 'Despesa' : 'Receita'}</td>
              <td>{t.pessoa?.nome}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}