using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Fretefy.Test.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _service;

        public RegiaoController(IRegiaoService service)
        {
            _service = service;
        }

        // GET /api/regiao?terms=...
        [HttpGet]
        public IActionResult List([FromQuery] string terms = null)
        {
            if (!string.IsNullOrWhiteSpace(terms))
                return Ok(_service.Query(terms));

            return Ok(_service.List());
        }

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
                // seu service lança ArgumentException quando não encontra
                return NotFound(new { mensagem = ex.Message, id });
            }
        }

        // Tipos mínimos de request (somente no WebAPI)
        public class CreateRegiaoRequest
        {
            public string Nome { get; set; }
            public IEnumerable<Guid> CidadeIds { get; set; }
        }

        public class UpdateRegiaoRequest
        {
            public string Nome { get; set; }
            public IEnumerable<Guid> CidadeIds { get; set; }
        }

        // POST /api/regiao
        [HttpPost]
        public IActionResult Create([FromBody] CreateRegiaoRequest body)
        {
            if (body == null)
                return BadRequest(new { mensagem = "Corpo da requisição inválido." });

            try
            {
                var created = _service.Create(body.Nome, body.CidadeIds);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PUT /api/regiao/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateRegiaoRequest body)
        {
            if (body == null)
                return BadRequest(new { mensagem = "Corpo da requisição inválido." });

            try
            {
                var updated = _service.Update(id, body.Nome, body.CidadeIds);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                // Pode ser nome inválido, duplicado, cidade inexistente OU região não encontrada
                // O service usa ArgumentException para ambos os casos
                // Se quiser diferenciar 404 de 400, podemos ajustar o service para lançar KeyNotFoundException quando não encontra
                return BadRequest(new { mensagem = ex.Message });
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
                // Se o repo/service jogar ArgumentException quando não existir
                return NotFound(new { mensagem = ex.Message, id });
            }
        }
    }
}
