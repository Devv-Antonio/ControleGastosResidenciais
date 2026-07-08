import { useEffect, useState } from 'react';
import type { FormEvent } from 'react';
import { api } from '../services/api';

interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}

export default function Pessoas() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  
  // Novos estados para UX (Experiência do Usuário)
  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState('');

  useEffect(() => {
    carregarPessoas();
  }, []);

  async function carregarPessoas() {
    try {
      const response = await api.get('/pessoas');
      setPessoas(response.data);
    } catch (error) {
      setErro('Erro ao carregar a lista de pessoas.');
    }
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setErro(''); // Limpa erros anteriores
    setLoading(true);

    try {
      await api.post('/pessoas', { nome, idade: Number(idade) });
      setNome('');
      setIdade('');
      await carregarPessoas();
    } catch (error: any) {
      // Pega a mensagem de erro que vem do nosso Back-end profissional
      setErro(error.response?.data?.message || 'Erro ao cadastrar pessoa. Verifique os dados.');
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete(id: number) {
    // Alerta de confirmação exigido por boas práticas!
    if (!window.confirm('Tem certeza? Isso apagará TODAS as transações desta pessoa!')) {
      return; 
    }

    try {
      await api.delete(`/pessoas/${id}`);
      await carregarPessoas();
    } catch (error) {
      setErro('Erro ao deletar pessoa.');
    }
  }

  return (
    <div className="container">
      <h1 className="title">Gestão de Pessoas</h1>

      {/* Exibe o erro na tela se ele existir */}
      {erro && <div className="alert-error">{erro}</div>}

      <div className="card">
        <form onSubmit={handleSubmit} className="form-group">
          <div className="input-field">
            <label>Nome Completo</label>
            <input
              type="text"
              value={nome}
              onChange={(e) => setNome(e.target.value)}
              required
              maxLength={100}
            />
          </div>
          
          <div className="input-field">
            <label>Idade</label>
            <input
              type="number"
              value={idade}
              onChange={(e) => setIdade(e.target.value)}
              required
              min="0"
              max="130"
            />
          </div>

          <button type="submit" className="btn" disabled={loading}>
            {loading ? 'Salvando...' : 'Cadastrar'}
          </button>
        </form>
      </div>

      <div className="card">
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Nome</th>
                <th>Idade</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {pessoas.map((pessoa) => (
                <tr key={pessoa.id}>
                  <td>{pessoa.id}</td>
                  <td>{pessoa.nome}</td>
                  <td>{pessoa.idade} anos</td>
                  <td>
                    <button 
                      onClick={() => handleDelete(pessoa.id)} 
                      className="btn btn-danger"
                    >
                      Deletar
                    </button>
                  </td>
                </tr>
              ))}
              {/* Mensagem amigável quando a tabela está vazia */}
              {pessoas.length === 0 && (
                <tr>
                  <td colSpan={4} style={{ textAlign: 'center', color: 'var(--text-muted)' }}>
                    Nenhuma pessoa cadastrada no momento.
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