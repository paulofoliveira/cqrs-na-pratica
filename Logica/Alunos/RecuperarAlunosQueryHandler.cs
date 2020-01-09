using Dapper;
using Logica.Models;
using Logica.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Logica.Alunos
{
    public sealed class RecuperarAlunosQueryHandler : IQueryHandler<RecuperarAlunosQuery, List<AlunoDto>>
    {
        private readonly QueriesConnectionString _connectionString;

        public RecuperarAlunosQueryHandler(QueriesConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AlunoDto> Handle(RecuperarAlunosQuery query)
        {
            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var sql = @"SELECT a.[AlunoID] AS [Id], 
                            a.[Nome], 
                            a.[Email], 
                            a.[Curso1Nome], 
                            a.[Curso1Grade], 
                            a.[Curso1Creditos], 
                            a.[Curso2Nome], 
                            a.[Curso2Grade], 
                            a.[Curso2Creditos]
                            FROM [Aluno] a
                            WHERE (a.[Curso1Nome] = @CursoNome OR a.[Curso2Nome] = @CursoNome OR @CursoNome IS NULL)
                            AND (a.[NumeroDeInscricoes] = @Numero OR @Numero IS NULL)
                            ORDER BY [Id] ASC";

                var alunos = conn.Query<AlunoDto>(sql, new
                {
                    query.CursoNome,
                    query.Numero
                });

                return alunos.ToList();
            }
        }
    }
}
