import React, { useState, useEffect } from 'react';

interface Solicitacao {
  id: number;
  alunoNome: string;
  matricula: string;
  tipoDocumento: string;
  status: string;
  prazoLimite: string;
  atrasada: boolean;
}

function App() {
  const [solicitacoes, setSolicitacoes] = useState<Solicitacao[]>([]);
  const [statusFilter, setStatusFilter] = useState('');
  const [apenasAtrasadas, setApenasAtrasadas] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [novoSolicitacao, setNovoSolicitacao] = useState({ alunoId: '', tipoDocumento: 'Declaração de Matrícula' });
  const [errorMessage, setErrorMessage] = useState('');

  const apiUrl = 'http://localhost:5103/api/solicitacoes';

  const carregarSolicitacoes = async () => {
    let url = apiUrl;
    const params = new URLSearchParams();
    if (statusFilter) params.append('status', statusFilter);
    if (apenasAtrasadas) params.append('apenasAtrasadas', 'true');
    if (params.toString()) url += '?' + params.toString();

    const resp = await fetch(url);
    const data = await resp.json();
    setSolicitacoes(data);
  };

  useEffect(() => {
    carregarSolicitacoes();
  }, [statusFilter, apenasAtrasadas]);

  const criarSolicitacao = async () => {
    setErrorMessage('');
    try {
      const resp = await fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          alunoId: parseInt(novoSolicitacao.alunoId),
          tipoDocumento: novoSolicitacao.tipoDocumento
        })
      });

      if (!resp.ok) {
        const error = await resp.json();
        throw new Error(error.message || 'Erro ao criar solicitação');
      }

      setShowModal(false);
      carregarSolicitacoes();
    } catch (err: any) {
      setErrorMessage(err.message);
    }
  };

  const atualizarStatus = async (id: number, novoStatus: string) => {
    setErrorMessage('');
    try {
      const resp = await fetch(`${apiUrl}/${id}/status`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ status: novoStatus })
      });

      if (!resp.ok) {
        const error = await resp.json();
        throw new Error(error.message || 'Erro ao atualizar status');
      }

      carregarSolicitacoes();
    } catch (err: any) {
      setErrorMessage(err.message);
    }
  };

  const formatarData = (data: string) => {
    return new Date(data).toLocaleDateString('pt-BR');
  };

  return (
    <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
      <h1>Solicitações de Documentos</h1>

      {errorMessage && (
        <div style={{ background: '#ffdddd', padding: '10px', marginBottom: '20px', border: '1px solid red' }}>
          <strong>Erro:</strong> {errorMessage}
        </div>
      )}

      <div style={{ marginBottom: '20px', display: 'flex', gap: '10px', alignItems: 'center' }}>
        <label>
          Status:
          <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
            <option value="">Todos</option>
            <option value="Pendente">Pendente</option>
            <option value="Concluída">Concluída</option>
            <option value="Cancelada">Cancelada</option>
          </select>
        </label>

        <label>
          <input
            type="checkbox"
            checked={apenasAtrasadas}
            onChange={(e) => setApenasAtrasadas(e.target.checked)}
          />
          Apenas atrasadas
        </label>

        <button onClick={() => setShowModal(true)} style={{ padding: '8px 16px', background: '#007bff', color: 'white', border: 'none', cursor: 'pointer' }}>
          Nova Solicitação
        </button>
      </div>

      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ background: '#f0f0f0' }}>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Aluno</th>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Matrícula</th>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Tipo</th>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Status</th>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Prazo</th>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Atrasada</th>
            <th style={{ padding: '8px', textAlign: 'left', border: '1px solid #ddd' }}>Ações</th>
          </tr>
        </thead>
        <tbody>
          {solicitacoes.map((s) => (
            <tr key={s.id}>
              <td style={{ padding: '8px', border: '1px solid #ddd' }}>{s.alunoNome}</td>
              <td style={{ padding: '8px', border: '1px solid #ddd' }}>{s.matricula}</td>
              <td style={{ padding: '8px', border: '1px solid #ddd' }}>{s.tipoDocumento}</td>
              <td style={{ padding: '8px', border: '1px solid #ddd' }}>{s.status}</td>
              <td style={{ padding: '8px', border: '1px solid #ddd' }}>{formatarData(s.prazoLimite)}</td>
              <td style={{ padding: '8px', border: '1px solid #ddd', color: s.atrasada ? 'red' : 'green' }}>
                {s.atrasada ? '✅' : '❌'}
              </td>
              <td style={{ padding: '8px', border: '1px solid #ddd' }}>
                {s.status === 'Pendente' && (
                  <>
                    <button onClick={() => atualizarStatus(s.id, 'Concluída')} style={{ marginRight: '5px' }}>
                      Concluir
                    </button>
                    <button onClick={() => atualizarStatus(s.id, 'Cancelada')}>
                      Cancelar
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showModal && (
        <div style={{ position: 'fixed', top: '0', left: '0', width: '100%', height: '100%', background: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
          <div style={{ background: 'white', padding: '20px', borderRadius: '8px', minWidth: '300px' }}>
            <h2>Nova Solicitação</h2>
            <div style={{ marginBottom: '10px' }}>
              <label>ID do Aluno:</label>
              <input
                type="number"
                value={novoSolicitacao.alunoId}
                onChange={(e) => setNovoSolicitacao({ ...novoSolicitacao, alunoId: e.target.value })}
                style={{ width: '100%', padding: '8px', marginTop: '5px' }}
              />
            </div>
            <div style={{ marginBottom: '10px' }}>
              <label>Tipo de Documento:</label>
              <select
                value={novoSolicitacao.tipoDocumento}
                onChange={(e) => setNovoSolicitacao({ ...novoSolicitacao, tipoDocumento: e.target.value })}
                style={{ width: '100%', padding: '8px', marginTop: '5px' }}
              >
                <option value="Declaração de Matrícula">Declaração de Matrícula</option>
                <option value="Atestado de Frequência">Atestado de Frequência</option>
                <option value="Histórico Escolar">Histórico Escolar</option>
              </select>
            </div>
            <div style={{ display: 'flex', gap: '10px', justifyContent: 'flex-end' }}>
              <button onClick={() => setShowModal(false)}>Cancelar</button>
              <button onClick={criarSolicitacao} style={{ background: '#007bff', color: 'white', border: 'none', padding: '8px 16px' }}>
                Criar
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;