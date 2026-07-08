import { useEffect, useState } from 'react';
import type { FormEvent } from 'react';
import { api } from '../services/api';

interface Pessoa {
  id: number;
  nome: string;
}

interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: number;
  pessoaId: number;
}

export default function Transacoes() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  
  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState('0');
  const [pessoaId, setPessoaId] = useState('');

  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState('');

  useEffect(() => {
    carregarDados();
  }, []);

  async function carregarDados() {
    try {
      const [resPessoas, resTransacoes] = await Promise.all([
        api.get('/pessoas'),
        api.get('/transacoes')
      ]);
      
      // Correção aplicada: acessando .items na resposta das pessoas
      setPessoas(resPessoas.data.items);
      setTransacoes(resTransacoes.data);
    } catch (error) {
      setErro('Erro ao carregar os dados iniciais.');
    }
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setErro('');
    setLoading(true);

    if (!pessoaId) {
      setErro('Selecione uma pessoa para a transação.');
      setLoading(false);
      return;
    }

    try {
      await api.post('/transacoes', {
        descricao,
        valor: Number(valor),
        tipo: Number(tipo),
        pessoaId: Number(pessoaId)
      });
      
      setDescricao('');
      setValor('');
      await carregarDados();
    } catch (error: any) {
      setErro(error.response?.data?.message || 'Erro ao cadastrar transação. Verifique os dados.');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="container">
      <h1 className="title">Lançamento de Transações</h1>

      {erro && <div className="alert-error">{erro}</div>}

      <div className="card">
        <form onSubmit={handleSubmit} className="form-group">
          <div className="input-field">
            <label>Pessoa</label>
            <select 
              value={pessoaId} 
              onChange={(e) => setPessoaId(e.target.value)} 
              required
            >
              <option value="">-- Selecione --</option>
              {pessoas.map(p => (
                <option key={p.id} value={p.id}>{p.nome}</option>
              ))}
            </select>
          </div>

          <div className="input-field">
            <label>Descrição</label>
            <input
              type="text"
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
              required
              maxLength={200}
              placeholder="Ex: Conta de Luz"
            />
          </div>
          
          <div className="input-field">
            <label>Valor (R$)</label>
            <input
              type="number"
              value={valor}
              onChange={(e) => setValor(e.target.value)}
              required
              min="0.01"
              step="0.01"
              placeholder="0.00"
            />
          </div>

          <div className="input-field">
            <label>Tipo</label>
            <select value={tipo} onChange={(e) => setTipo(e.target.value)}>
              <option value="0">Despesa</option>
              <option value="1">Receita</option>
            </select>
          </div>

          <button type="submit" className="btn" disabled={loading}>
            {loading ? 'Salvando...' : 'Lançar'}
          </button>
        </form>
      </div>

      <div className="card">
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Descrição</th>
                <th>Tipo</th>
                <th>Valor</th>
                <th>ID Pessoa</th>
              </tr>
            </thead>
            <tbody>
              {transacoes.map((t) => (
                <tr key={t.id}>
                  <td>{t.id}</td>
                  <td>{t.descricao}</td>
                  <td>
                    <span style={{ 
                      color: t.tipo === 1 ? 'var(--success-color)' : 'var(--danger-color)',
                      fontWeight: 500
                    }}>
                      {t.tipo === 1 ? 'Receita' : 'Despesa'}
                    </span>
                  </td>
                  <td>
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(t.valor)}
                  </td>
                  <td>{t.pessoaId}</td>
                </tr>
              ))}
              {transacoes.length === 0 && (
                <tr>
                  <td colSpan={5} style={{ textAlign: 'center', color: 'var(--text-muted)' }}>
                    Nenhuma transação lançada ainda.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}