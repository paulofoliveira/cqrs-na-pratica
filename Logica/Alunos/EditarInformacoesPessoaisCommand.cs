﻿namespace Logica.Alunos
{
    public sealed class EditarInformacoesPessoaisCommand : ICommand
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
