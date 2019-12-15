using CSharpFunctionalExtensions;
using Logica.Decorators;
using Logica.Utils;
using System;

namespace Logica.Alunos
{
    public sealed class InscreverCursoCommand : ICommand
    {
        public InscreverCursoCommand(long id, string curso, string grade)
        {
            Id = id;
            Curso = curso;
            Grade = grade;
        }
        public long Id { get; }
        public string Curso { get; }
        public string Grade { get; }
    }

    [DatabaseRetry]
    public sealed class InscreverCursoCommandHandler : ICommandHandler<InscreverCursoCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public InscreverCursoCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(InscreverCursoCommand command)
        {
            var uow = new UnitOfWork(_sessionFactory);

            var alunoRepositorio = new AlunoRepositorio(uow);
            var cursoRepositorio = new CursoRepositorio(uow);

            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            var curso = cursoRepositorio.RecuperarPorNome(command.Curso);

            if (curso == null)
                return Result.Fail($"O curso é incorreto: {command.Curso}.");

            var gradeSucesso = Enum.TryParse(command.Grade, out Grade grade);

            if (!gradeSucesso)
                return Result.Fail($"A grade é incorreta: {command.Grade}.");

            aluno.Inscrever(curso, grade);
            uow.Commit();

            return Result.Ok();
        }
    }
}
