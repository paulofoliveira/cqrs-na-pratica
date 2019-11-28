using Logica.Models;
using System.Collections.Generic;

namespace Logica.Alunos
{
    public sealed class RecuperarAlunosQuery : IQuery<List<AlunoDto>>
    {
        public RecuperarAlunosQuery(string cursoNome, int? numero)
        {
            CursoNome = cursoNome;
            Numero = numero;
        }

        public string CursoNome { get; set; }
        public int? Numero { get; set; }
    }
}
