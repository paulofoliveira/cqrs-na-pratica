using CSharpFunctionalExtensions;
using Logica.Utils;

namespace Logica.Alunos
{
    public sealed class EditarInformacoesPessoaisCommandHandler : ICommandHandler<EditarInformacoesPessoaisCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public EditarInformacoesPessoaisCommandHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(EditarInformacoesPessoaisCommand command)
        {
            var alunoRepositorio = new AlunoRepositorio(_unitOfWork);

            var aluno = alunoRepositorio.RecuperarPorId(command.Id);

            if (aluno == null)
                return Result.Fail($"Nenhum aluno encontrado com o Id {command.Id}");

            aluno.Nome = command.Nome;
            aluno.Email = command.Email;

            _unitOfWork.Commit();

            return Result.Ok();
        }
    }
}
