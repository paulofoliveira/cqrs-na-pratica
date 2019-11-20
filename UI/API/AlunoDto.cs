using System;

namespace UI.API
{
    public sealed class AlunoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Curso1 { get; set; }
        public string Curso1Grade { get; set; }
        public int? Curso1Creditos { get; set; }
        public string Curso2 { get; set; }
        public string Curso2Grade { get; set; }   
        public int? Curso2Creditos { get; set; }
    }
    public class Envelope<T>
    {
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TimeGenerated { get; set; }
    }

    public sealed class AlunoInformacoesPessoaisDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public sealed class AlunoInscricaoDto
    {
        public long Id { get; set; }
        public string Curso { get; set; }
        public string Grade { get; set; }
    }

    public class AlunoTransferenciaDto
    {
        public long Id { get; set; }
        public string Curso { get; set; }
        public string Grade { get; set; }
        public int NumeroInscricao { get; set; }
    }

    public sealed class NovoAlunoDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Curso1 { get; set; }
        public string Curso1Grade { get; set; }
        public string Curso2 { get; set; }
        public string Curso2Grade { get; set; }
    }

    public sealed class AlunoDesinscricaoDto
    {
        public long Id { get; set; }
        public string Comentario { get; set; }
        public int NumeroInscricao { get; set; }
    }

}
