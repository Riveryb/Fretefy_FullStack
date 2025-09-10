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
                       .Include(r => r.RegiaoCidades).ThenInclude(rc => rc.Cidade)
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
                       .Include(r => r.RegiaoCidades).ThenInclude(rc => rc.Cidade)
                       .AsNoTracking()
                       .OrderBy(r => r.Nome)
                       .ToList();
        }

        public IEnumerable<Regiao> Query(string terms)
        {
            terms = (terms ?? "").Trim();
            if (string.IsNullOrEmpty(terms)) return Enumerable.Empty<Regiao>();

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

            var q = _ctx.Regioes.AsNoTracking().Where(r => r.Nome.ToLower() == norm);
            if (ignoreId.HasValue) q = q.Where(r => r.Id != ignoreId.Value);
            return q.Any();
        }

        public override Regiao Update(Regiao entidade)
        {
            // Segurança: garante que ninguém está rastreado
            _ctx.ChangeTracker.Clear();

            using var tx = _ctx.Database.BeginTransaction();

            // Verifica se existe
            var exists = _ctx.Regioes.AsNoTracking().Any(r => r.Id == entidade.Id);
            if (!exists) throw new ArgumentException("Região não encontrada.");

            // 1) Atualiza a REGIÃO via SQL (sem rastrear)
            _ctx.Database.ExecuteSqlRaw(
                "UPDATE Regiao SET Nome = {0}, Ativo = {1} WHERE Id = {2}",
                entidade.Nome, entidade.Ativo, entidade.Id);

            // 2) Limpa TODOS os vínculos (sem rastrear)
            _ctx.Database.ExecuteSqlRaw(
                "DELETE FROM RegiaoCidade WHERE RegiaoId = {0}", entidade.Id);

            // 3) (Re)insere vínculos (apenas INSERTs -> não disparam concurrency)
            var cidadeIds = (entidade.RegiaoCidades ?? new List<RegiaoCidade>())
                            .Select(rc => rc.CidadeId)
                            .Where(id => id != Guid.Empty)
                            .Distinct()
                            .ToList();

            foreach (var cid in cidadeIds)
                _ctx.RegiaoCidades.Add(new RegiaoCidade(entidade.Id, cid));

            _ctx.SaveChanges();
            tx.Commit();

            // Retorna já populado
            return _ctx.Regioes
                    .Include(r => r.RegiaoCidades)
                        .ThenInclude(rc => rc.Cidade)
                    .AsNoTracking()
                    .First(r => r.Id == entidade.Id);
        }
    }
}
