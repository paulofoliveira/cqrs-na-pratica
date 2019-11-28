using CSharpFunctionalExtensions;

namespace Logica.Alunos
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}
