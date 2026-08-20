using Microsoft.EntityFrameworkCore;
using UbecSolicitacoes.Data;
using UbecSolicitacoes.Models;
using UbecSolicitacoes.DTOs;

namespace UbecSolicitacoes.Services
{
    public class SolicitacaoService
    {
        private readonly AppDbContext _context;

        public SolicitacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SolicitacaoResponseDTO>> ListarAsync(string? status, bool? apenasAtrasadas)
        {
            var query = _context.Solicitacoes
                .Include(s => s.Aluno)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(s => s.Status == status);

            var lista = await query.ToListAsync();

            var resultado = lista.Select(s => new SolicitacaoResponseDTO
            {
                Id = s.Id,
                AlunoNome = s.Aluno?.Nome ?? "",
                Matricula = s.Aluno?.Matricula ?? "",
                TipoDocumento = s.TipoDocumento,
                Status = s.Status,
                PrazoLimite = s.PrazoLimite,
                Atrasada = s.Status == "Pendente" && DateTime.Now > s.PrazoLimite
            }).ToList();

            if (apenasAtrasadas == true)
                resultado = resultado.Where(s => s.Atrasada).ToList();

            return resultado;
        }

        public async Task<Solicitacao> CriarAsync(SolicitacaoCreateDTO dto)
        {
            var aluno = await _context.Alunos.FindAsync(dto.AlunoId);
            if (aluno == null)
                throw new BusinessException("Aluno não encontrado.", 404);
            
            if (!aluno.Ativo)
                throw new BusinessException("Aluno inativo não pode abrir solicitação.", 400);

            var pendente = await _context.Solicitacoes
                .AnyAsync(s => s.AlunoId == dto.AlunoId 
                              && s.TipoDocumento == dto.TipoDocumento 
                              && s.Status == "Pendente");
            
            if (pendente)
                throw new BusinessException("Já existe uma solicitação pendente do mesmo tipo para este aluno.", 409);

            var prazoDias = GetPrazoDias(dto.TipoDocumento);
            var solicitacao = new Solicitacao
            {
                AlunoId = dto.AlunoId,
                TipoDocumento = dto.TipoDocumento,
                Status = "Pendente",
                CriadaEm = DateTime.Now,
                PrazoLimite = DateTime.Now.AddDays(prazoDias)
            };

            _context.Solicitacoes.Add(solicitacao);
            await _context.SaveChangesAsync();
            return solicitacao;
        }

        public async Task AtualizarStatusAsync(int id, SolicitacaoStatusUpdateDTO dto)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null)
                throw new BusinessException("Solicitação não encontrada.", 404);

            if (solicitacao.Status != "Pendente")
                throw new BusinessException("Apenas solicitações Pendentes podem ser concluídas ou canceladas.", 400);

            if (dto.Status != "Concluída" && dto.Status != "Cancelada")
                throw new BusinessException("Status inválido. Use 'Concluída' ou 'Cancelada'.", 400);

            solicitacao.Status = dto.Status;
            await _context.SaveChangesAsync();
        }

        private int GetPrazoDias(string tipo)
        {
            return tipo switch
            {
                "Declaração de Matrícula" => 2,
                "Atestado de Frequência" => 3,
                "Histórico Escolar" => 5,
                _ => throw new BusinessException("Tipo de documento inválido.", 400)
            };
        }
    }

    public class BusinessException : Exception
    {
        public int StatusCode { get; set; }
        public BusinessException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}