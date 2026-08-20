namespace UbecSolicitacoes.Models
{
    public class Solicitacao
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente";
        public DateTime CriadaEm { get; set; }
        public DateTime PrazoLimite { get; set; }
        public Aluno? Aluno { get; set; }
    }
}