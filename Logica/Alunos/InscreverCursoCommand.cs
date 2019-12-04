using CSharpFunctionalExtensions;
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

    public sealed class InscreverCursoCommandHandler : ICommandHandler<InscreverCursoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public InscreverCursoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(InscreverCursoCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);
            var cursoRepositorio = new CursoRepositorio(_unitOfWork);

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
            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
