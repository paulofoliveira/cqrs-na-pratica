using CSharpFunctionalExtensions;
using Logica.Decorators;
using Logica.Utils;

namespace Logica.Alunos
{
    public sealed class EditarInformacoesPessoaisCommand : ICommand
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    [DatabaseRetry]
    public sealed class EditarInformacoesPessoaisCommandHandler : ICommandHandler<EditarInformacoesPessoaisCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public EditarInformacoesPessoaisCommandHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(EditarInformacoesPessoaisCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);
            var alunoRepositorio = new AlunoRepositorio(unitOfWork);

            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            aluno.Nome = command.Nome;
            aluno.Email = command.Email;

            unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
