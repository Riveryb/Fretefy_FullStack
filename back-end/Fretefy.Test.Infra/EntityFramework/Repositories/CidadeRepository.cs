using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class CidadeRepository : Repository<Cidade>, ICidadeRepository
    {
        public CidadeRepository(TestDbContext ctx) : base(ctx) { }

        public IEnumerable<Cidade> ListByUf(string uf)
        {
            uf = (uf ?? "").Trim().ToUpperInvariant();

            return _set.AsNoTracking()
                       .Where(c => c.UF == uf)
                       .OrderBy(c => c.Nome)
                       .ToList();
        }

        public IEnumerable<Cidade> Query(string terms)
        {
            terms = (terms ?? "").Trim();
            if (string.IsNullOrEmpty(terms))
                return Enumerable.Empty<Cidade>();

            return _set.AsNoTracking()
                       .Where(c => EF.Functions.Like(c.Nome, $"%{terms}%"))
                       .OrderBy(c => c.Nome)
                       .ToList();
        }
    }
}
