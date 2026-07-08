import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
        <header style={{ marginBottom: '20px' }}>
          <h1>Controle de Gastos Residenciais</h1>
          <nav style={{ display: 'flex', gap: '15px' }}>
            <Link to="/">Dashboard (Totais)</Link>
            <Link to="/pessoas">Pessoas</Link>
            <Link to="/transacoes">Transações</Link>
          </nav>
        </header>

        <main>
          <Routes>
            <Route path="/" element={<h2>Página de Totais em construção...</h2>} />
            <Route path="/pessoas" element={<h2>Página de Pessoas em construção...</h2>} />
            <Route path="/transacoes" element={<h2>Página de Transações em construção...</h2>} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;