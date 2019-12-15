﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Logica.Alunos
{
    public class Aluno : Entidade
    {
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }

        private readonly IList<Inscricao> _inscricoes = new List<Inscricao>();
        public virtual IReadOnlyList<Inscricao> Inscricoes => _inscricoes.ToList();

        private readonly IList<Desinscricao> _desincricoes = new List<Desinscricao>();
        public virtual IReadOnlyList<Desinscricao> Desincricoes => _desincricoes.ToList();

        protected Aluno()
        {
        }

        public Aluno(string nome, string email)
            : this()
        {
            Nome = nome;
            Email = email;
        }

        public virtual Inscricao RecuperarInscricao(int index)
        {
            index--;

            if (_inscricoes.Count > index)
                return _inscricoes[index];

            return null;
        }

        public virtual void RemoverInscricao(Inscricao inscricao, string comentario)
        {
            _inscricoes.Remove(inscricao);

            var desincricao = new Desinscricao(inscricao.Aluno, inscricao.Curso, comentario);
            _desincricoes.Add(desincricao);
        }

        public virtual void Inscrever(Curso curso, Grade grade)
        {
            if (_inscricoes.Count >= 2)
                throw new Exception("Não pode ter mais de 2 inscrições.");

            var inscricao = new Inscricao(this, curso, grade);
            _inscricoes.Add(inscricao);
        }
    }
}
