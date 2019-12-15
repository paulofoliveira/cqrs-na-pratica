using API.Models;
using Logica.Alunos;
using Logica.Utils;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/alunos")]
    public sealed class AlunoController : BaseController
    {
        private readonly Messages _messages;

        public AlunoController(Messages messages)
        {
            _messages = messages;
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
            var registrarCommand = new RegistrarAlunoCommand(dto.Nome, dto.Email, dto.Curso1, dto.Curso1Grade, dto.Curso2, dto.Curso2Grade);

            var result = _messages.Dispatch(registrarCommand);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public IActionResult Desregistrar(long id)
        {
            var desregistrarCommand = new DesregistrarAlunoCommand(id);

            var result = _messages.Dispatch(desregistrarCommand);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{id}/inscricoes")]
        public IActionResult Inscrever(long id, [FromBody]AlunoInscricaoDto dto)
        {
            var inscreverCursoCommand = new InscreverCursoCommand(id, dto.Curso, dto.Grade);

            var result = _messages.Dispatch(inscreverCursoCommand);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}/inscricoes/{numeroInscricao}")]
        public IActionResult Transferir(long id, int numeroInscricao, [FromBody]AlunoTransferenciaDto dto)
        {
            var transferirCommand = new TransferirCursoCommand(id, numeroInscricao, dto.Curso, dto.Grade);

            var result = _messages.Dispatch(transferirCommand);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{id}/inscricoes/{numeroInscricao}/excluir")]
        public IActionResult Desinscrever(long id, int numeroInscricao, [FromBody]AlunoDesinscricaoDto dto)
        {
            var desinscreverCursoCommand = new DesinscreverCursoCommand(id, numeroInscricao, dto.Comentario);

            var result = _messages.Dispatch(desinscreverCursoCommand);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
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
