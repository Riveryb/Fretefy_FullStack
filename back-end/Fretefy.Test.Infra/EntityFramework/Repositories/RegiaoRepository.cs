using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class RegiaoRepository : Repository<Regiao>, IRegiaoRepository
    {
        public RegiaoRepository(TestDbContext ctx) : base(ctx) { }

        public Regiao GetWithCidades(Guid id)
        {
            return _ctx.Regioes
                       .Include(r => r.RegiaoCidades)
                           .ThenInclude(rc => rc.Cidade)
                       .AsNoTracking()
                       .FirstOrDefault(r => r.Id == id);
        }

        public Regiao GetWithCidadesTracked(Guid id)
        {
            return _ctx.Regioes
                    .Include(r => r.RegiaoCidades)
                    .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Regiao> ListWithCidades()
        {
            return _ctx.Regioes
                       .Include(r => r.RegiaoCidades)
                           .ThenInclude(rc => rc.Cidade)
                       .AsNoTracking()
                       .OrderBy(r => r.Nome)
                       .ToList();
        }

        public IEnumerable<Regiao> Query(string terms)
        {
            terms = (terms ?? "").Trim();
            if (string.IsNullOrEmpty(terms))
                return Enumerable.Empty<Regiao>();

            return _ctx.Regioes
                       .AsNoTracking()
                       .Where(r => EF.Functions.Like(r.Nome, $"%{terms}%"))
                       .OrderBy(r => r.Nome)
                       .ToList();
        }

        public IEnumerable<Regiao> List(bool? ativo = null)
        {
            var q = _ctx.Regioes.AsNoTracking();
            if (ativo.HasValue) q = q.Where(r => r.Ativo == ativo.Value);
            return q.OrderBy(r => r.Nome).ToList();
        }

        public bool ExistsByName(string nome, Guid? ignoreId = null)
        {
            if (string.IsNullOrWhiteSpace(nome)) return false;
            var norm = nome.Trim().ToLower();

            var q = _ctx.Regioes.AsNoTracking()
                .Where(r => r.Nome.ToLower() == norm);

            if (ignoreId.HasValue)
                q = q.Where(r => r.Id != ignoreId.Value);

            return q.Any();
        }
    }
}
