import { useEffect, useState, type FormEvent } from 'react';
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

  useEffect(() => {
    carregarPessoas();
  }, []);

  async function carregarPessoas() {
    const response = await api.get('/pessoas');
    setPessoas(response.data);
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    await api.post('/pessoas', { nome, idade: Number(idade) });
    setNome('');
    setIdade('');
    carregarPessoas();
  }

  async function handleDelete(id: number) {
    await api.delete(`/pessoas/${id}`);
    carregarPessoas();
  }

  return (
    <div>
      <h2>Cadastro de Pessoas</h2>
      
      <form onSubmit={handleSubmit} style={{ marginBottom: '20px' }}>
        <input 
          type="text" 
          placeholder="Nome" 
          value={nome} 
          onChange={e => setNome(e.target.value)} 
          required 
          style={{ marginRight: '10px' }}
        />
        <input 
          type="number" 
          placeholder="Idade" 
          value={idade} 
          onChange={e => setIdade(e.target.value)} 
          required 
          style={{ marginRight: '10px' }}
        />
        <button type="submit">Cadastrar</button>
      </form>

      <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%' }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nome</th>
            <th>Idade</th>
            <th>Ação</th>
          </tr>
        </thead>
        <tbody>
          {pessoas.map(pessoa => (
            <tr key={pessoa.id}>
              <td>{pessoa.id}</td>
              <td>{pessoa.nome}</td>
              <td>{pessoa.idade}</td>
              <td>
                <button onClick={() => handleDelete(pessoa.id)}>Deletar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}