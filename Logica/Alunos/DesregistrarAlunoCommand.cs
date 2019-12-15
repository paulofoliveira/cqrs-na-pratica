using CSharpFunctionalExtensions;
using Logica.Decorators;
using Logica.Utils;

namespace Logica.Alunos
{
    public sealed class DesregistrarAlunoCommand : ICommand
    {
        public DesregistrarAlunoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    [DatabaseRetry]
    public sealed class DesregistrarAlunoCommandHandler : ICommandHandler<DesregistrarAlunoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public DesregistrarAlunoCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(DesregistrarAlunoCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);

            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            alunoRepositorio.Excluir(aluno);
            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
