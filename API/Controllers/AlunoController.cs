﻿using API.Models;
using Logica.Alunos;
using Logica.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/alunos")]
    public sealed class AlunoController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Messages _messages;
        private readonly AlunoRepositorio _alunoRepositorio;
        private readonly CursoRepositorio _cursoRepositorio;

        public AlunoController(UnitOfWork unitOfWork, Messages messages)
        {
            _unitOfWork = unitOfWork;
            _messages = messages;
            _alunoRepositorio = new AlunoRepositorio(unitOfWork);
            _cursoRepositorio = new CursoRepositorio(unitOfWork);
        }

        [HttpGet]
        public IActionResult GetLista(string cursoNome, int? numero)
        {
            var query = new RecuperarAlunosQuery(cursoNome, numero);
            var lista = _messages.Dispatch(query);

            return Ok(lista);
        }        

        [HttpPost]
        public IActionResult Registrar([FromBody] NovoAlunoDto dto)
        {
            var aluno = new Aluno(dto.Nome, dto.Email);

            if (dto.Curso1 != null && dto.Curso1Grade != null)
            {
                var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso1);
                aluno.Inscrever(curso, Enum.Parse<Grade>(dto.Curso1Grade));
            }

            if (dto.Curso2 != null && dto.Curso2Grade != null)
            {
                var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso2);
                aluno.Inscrever(curso, Enum.Parse<Grade>(dto.Curso2Grade));
            }

            _alunoRepositorio.Salvar(aluno);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Desregistrar(long id)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            _alunoRepositorio.Excluir(aluno);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost("{id}/inscricoes")]
        public IActionResult Inscrever(long id, [FromBody]AlunoInscricaoDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso);

            if (curso == null)
                return Error($"O curso é incorreto: {dto.Curso}.");

            var gradeSucesso = Enum.TryParse(dto.Grade, out Grade grade);

            if (!gradeSucesso)
                return Error($"A grade é incorreta: {dto.Grade}.");

            aluno.Inscrever(curso, grade);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}/inscricoes/{numeroInscricao}")]
        public IActionResult Transferir(long id, int numeroInscricao, [FromBody]AlunoTransferenciaDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            var curso = _cursoRepositorio.RecuperarPorNome(dto.Curso);

            if (curso == null)
                return Error($"O curso é incorreto: {dto.Curso}.");

            var gradeSucesso = Enum.TryParse(dto.Grade, out Grade grade);

            if (!gradeSucesso)
                return Error($"A grade é incorreta: {dto.Grade}.");

            var inscricao = aluno.RecuperarInscricao(numeroInscricao);

            if (inscricao == null)
                return Error($"Nenhuma inscrição encontrada com o número: {numeroInscricao}");

            inscricao.Atualizar(curso, grade);
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPost("{id}/inscricoes/{numeroInscricao}/excluir")]
        public IActionResult Desinscricao(long id, int numeroInscricao, [FromBody]AlunoDesinscricaoDto dto)
        {
            var aluno = _alunoRepositorio.RecuperarPorId(id);

            if (aluno == null)
                return Error($"Nenhum aluno encontrado com o Id {id}");

            if (string.IsNullOrWhiteSpace(dto.Comentario))
                return Error("Comentário de desinscrição é requerido.");

            var inscricao = aluno.RecuperarInscricao(numeroInscricao);

            if (inscricao == null)
                return Error($"Nenhuma inscrição encontrada com o número: {numeroInscricao}");
            aluno.RemoverInscricao(inscricao, dto.Comentario);

            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarInformacoesPessoais(long id, [FromBody]AlunoInformacoesPessoaisDto dto)
        {
            var command = new EditarInformacoesPessoaisCommand()
            {
                Id = id,
                Nome = dto.Nome,
                Email = dto.Email
            };
            
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
