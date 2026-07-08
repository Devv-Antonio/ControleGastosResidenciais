import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import Pessoas from './pages/Pessoas';
import Transacoes from './pages/Transacoes';

function App() {
  return (
    <BrowserRouter>
      <div style={{ padding: '20px', fontFamily: 'sans-serif', maxWidth: '1000px', margin: '0 auto' }}>
        <header style={{ marginBottom: '30px', borderBottom: '1px solid #ccc', paddingBottom: '10px' }}>
          <h1>Controle de Gastos Residenciais</h1>
          <nav style={{ display: 'flex', gap: '20px', fontWeight: 'bold' }}>
            <Link to="/" style={{ textDecoration: 'none', color: '#333' }}>Dashboard (Totais)</Link>
            <Link to="/pessoas" style={{ textDecoration: 'none', color: '#333' }}>Pessoas</Link>
            <Link to="/transacoes" style={{ textDecoration: 'none', color: '#333' }}>Transações</Link>
          </nav>
        </header>

        <main>
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/pessoas" element={<Pessoas />} />
            <Route path="/transacoes" element={<Transacoes />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;