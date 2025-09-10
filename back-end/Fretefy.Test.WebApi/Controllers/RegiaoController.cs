using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fretefy.Test.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _service;
        public RegiaoController(IRegiaoService service) => _service = service;

        // GET /api/regiao?terms=...
        [HttpGet]
        public IActionResult List([FromQuery] string terms = null, [FromQuery] string status = null)
        {
            bool? ativo = status?.ToLower() switch
            {
                "ativo" => true,
                "inativo" => false,
                _ => (bool?)null
            };

            if (!string.IsNullOrWhiteSpace(terms))
                return Ok(_service.Query(terms));

            // usa repositório com filtro de status
            return Ok(_service.List(ativo));
        }

        // GET /api/regiao/with-cidades
        [HttpGet("with-cidades")]
        public IActionResult ListWithCidades() => Ok(_service.ListWithCidades());

        // GET /api/regiao/{id}
        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var regiao = _service.Get(id);
                return Ok(regiao);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message, id });
            }
        }

        // DTOs do request (apenas na WebApi)
        public class RegiaoDto
        {
            public string Nome { get; set; }
            public IEnumerable<Guid> CidadeIds { get; set; }
        }

        // GET /api/regiao/exists?nome=Nordeste&ignoreId={guid?}
        [HttpGet("exists")]
        public IActionResult Exists([FromQuery] string nome, [FromQuery] Guid? ignoreId)
            => Ok(_service.ExistsByName(nome, ignoreId));

        // POST /api/regiao
        [HttpPost]
        public IActionResult Create([FromBody] RegiaoDto body)
        {
            if (body == null) return BadRequest(new { mensagem = "Corpo da requisição inválido." });

            try
            {
                var created = _service.Create(body.Nome, body.CidadeIds ?? Enumerable.Empty<Guid>());
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // regra de negócio já retornando conflito (nome duplicado)
                return Conflict(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                // conflito por índice UNIQUE (concorrência ou chamada direta)
                return Conflict(new { mensagem = "Já existe uma região com esse nome." });
            }
        }

        // PUT /api/regiao/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] RegiaoDto body)
        {
            if (body == null) return BadRequest(new { mensagem = "Corpo da requisição inválido." });

            try
            {
                var updated = _service.Update(id, body.Nome, body.CidadeIds ?? Enumerable.Empty<Guid>());
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                return Conflict(new { mensagem = "Já existe uma região com esse nome." });
            }
        }

        // DELETE /api/regiao/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message, id });
            }
        }

        // PUT /api/regiao/{id}/ativar
        [HttpPut("{id:guid}/ativar")]
        public IActionResult Ativar(Guid id)
        {
            try { return Ok(_service.Ativar(id)); }
            catch (ArgumentException ex) { return NotFound(new { mensagem = ex.Message, id }); }
        }

        // PUT /api/regiao/{id}/desativar
        [HttpPut("{id:guid}/desativar")]
        public IActionResult Desativar(Guid id)
        {
            try { return Ok(_service.Desativar(id)); }
            catch (ArgumentException ex) { return NotFound(new { mensagem = ex.Message, id }); }
        }

        // GET /api/regiao/export  -> CSV (abre no Excel)
        [HttpGet("export")]
        public IActionResult Export()
        {
            var regioes = _service.ListWithCidades();

            var sb = new StringBuilder();
            sb.AppendLine("Id;Nome;Ativo;Cidades");
            foreach (var r in regioes)
            {
                var cidades = string.Join(" | ", r.RegiaoCidades.Select(rc => $"{rc.Cidade.Nome}/{rc.Cidade.UF}"));
                sb.AppendLine($"{r.Id};{Esc(r.Nome)};{(r.Ativo ? "Ativo" : "Inativo")};{Esc(cidades)}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"regioes_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);

            static string Esc(string v)
            {
                if (string.IsNullOrEmpty(v)) return "";
                var need = v.Contains(';') || v.Contains('"') || v.Contains('\n') || v.Contains('\r');
                return need ? $"\"{v.Replace("\"", "\"\"")}\"" : v;
            }
        }        
        
        private static bool IsUniqueViolation(DbUpdateException ex)
        {
            // SQLite costuma trazer "UNIQUE constraint failed"
            var msg = ex.InnerException?.Message ?? ex.Message;
            return msg.IndexOf("UNIQUE", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
