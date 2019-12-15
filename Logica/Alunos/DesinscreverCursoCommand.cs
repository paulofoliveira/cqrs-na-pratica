using CSharpFunctionalExtensions;
using Logica.Decorators;
using Logica.Utils;

namespace Logica.Alunos
{
    public sealed class DesinscreverCursoCommand : ICommand
    {
        public DesinscreverCursoCommand(long id, int numeroInscricao, string comentario)
        {
            Id = id;
            NumeroInscricao = numeroInscricao;
            Comentario = comentario;
        }

        public long Id { get; }
        public int NumeroInscricao { get; }
        public string Comentario { get; }
    }

    [DatabaseRetry]
    public sealed class DesinscreverCursoCommandHandler : ICommandHandler<DesinscreverCursoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public DesinscreverCursoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(DesinscreverCursoCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);

            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            if (string.IsNullOrWhiteSpace(command.Comentario))
                return Result.Fail("Comentário de desinscrição é requerido.");

            var inscricao = aluno.RecuperarInscricao(command.NumeroInscricao);

            if (inscricao == null)
                return Result.Fail($"Nenhuma inscrição encontrada com o número: {command.NumeroInscricao}");
            aluno.RemoverInscricao(inscricao, command.Comentario);

            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
