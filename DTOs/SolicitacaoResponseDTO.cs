namespace UbecSolicitacoes.DTOs
{
    public class SolicitacaoResponseDTO
    {
        public int Id { get; set; }
        public string AlunoNome { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PrazoLimite { get; set; }
        public bool Atrasada { get; set; }
    }
}