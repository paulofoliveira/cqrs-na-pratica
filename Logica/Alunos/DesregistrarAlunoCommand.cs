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
        private readonly SessionFactory _sesionFactory;

        public DesregistrarAlunoCommandHandler(SessionFactory sessionFactory)
        {
            _sesionFactory = sessionFactory;
        }

        public Result Handle(DesregistrarAlunoCommand command)
        {
            var uow = new UnitOfWork(_sesionFactory);

            var alunoRepositorio = new AlunoRepositorio(uow);

            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            alunoRepositorio.Excluir(aluno);
            uow.Commit();

            return Result.Ok();
        }
    }
}
