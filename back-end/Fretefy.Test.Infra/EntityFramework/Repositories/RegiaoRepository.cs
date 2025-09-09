using System;
using System.Collections.Generic;
using System.Linq;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private static readonly List<Regiao> _store = new List<Regiao>();

        public IQueryable<Regiao> List() => _store.AsQueryable();
        public Regiao Get(Guid id) => _store.FirstOrDefault(r => r.Id == id);
        public void Add(Regiao r) => _store.Add(r);
        public void Update(Regiao r)
        {
            var i = _store.FindIndex(x => x.Id == r.Id);
            if (i < 0) throw new ArgumentException("Região não encontrada.");
            _store[i] = r;
        }
        public void Delete(Guid id)
        {
            var r = Get(id);
            if (r == null) throw new ArgumentException("Região não encontrada.");
            _store.Remove(r);
        }
        public IEnumerable<Regiao> Query(string terms)
        {
            if (string.IsNullOrWhiteSpace(terms)) return _store;
            terms = terms.Trim().ToLowerInvariant();
            return _store.Where(r => (r.Nome ?? "").ToLowerInvariant().Contains(terms));
        }
        public bool ExistsByName(string nome) =>
            _store.Any(r => string.Equals(r.Nome?.Trim(), (nome ?? "").Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
