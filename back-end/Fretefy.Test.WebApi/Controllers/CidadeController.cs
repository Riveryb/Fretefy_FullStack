using System;
using System.Collections.Generic;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fretefy.Test.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private readonly ICidadeService _service;
        public CidadeController(ICidadeService service) => _service = service;

        // GET /api/cidade?uf=PR&terms=Curit
        [HttpGet]
        public IActionResult List([FromQuery] string uf, [FromQuery] string terms)
        {
            IEnumerable<Cidade> cidades;
            if (!string.IsNullOrWhiteSpace(terms)) cidades = _service.Query(terms);
            else if (!string.IsNullOrWhiteSpace(uf)) cidades = _service.ListByUf(uf);
            else cidades = _service.List();

            return Ok(cidades);
        }

        // GET /api/cidade/{id}
        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var cidade = _service.Get(id);
                return Ok(cidade);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message, id });
            }
        }
    }
}
