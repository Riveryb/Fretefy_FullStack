using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Fretefy.Test.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fretefy.Test.Domain.Services
{
    public class RegiaoService : IRegiaoService
    {
        private readonly IRegiaoRepository _regiaoRepo;
        private readonly ICidadeRepository _cidadeRepo;

        public RegiaoService(IRegiaoRepository regiaoRepo, ICidadeRepository cidadeRepo)
        {
            _regiaoRepo = regiaoRepo;
            _cidadeRepo = cidadeRepo;
        }

        public Regiao Get(Guid id)
        {
            var regiao = _regiaoRepo.GetWithCidades(id);
            if (regiao == null) throw new ArgumentException("Região não encontrada.");
            return regiao;
        }

        public IEnumerable<Regiao> List() => _regiaoRepo.List();

        public IEnumerable<Regiao> List(bool? ativo) => _regiaoRepo.List(ativo);

        public IEnumerable<Regiao> ListWithCidades() => _regiaoRepo.ListWithCidades();

        public IEnumerable<Regiao> Query(string terms) => _regiaoRepo.Query(terms);

        public Regiao Create(string nome, IEnumerable<Guid> cidadeIds)
        {
            nome = (nome ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da região é obrigatório.");

            if (_regiaoRepo.Query(nome).Any(r => r.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Já existe uma região com esse nome.");

            var ids = (cidadeIds ?? Enumerable.Empty<Guid>())
                      .Where(id => id != Guid.Empty)
                      .Distinct()
                      .ToList();

            if (!ids.Any())
                throw new ArgumentException("Informe ao menos uma cidade.");

            var existentes = _cidadeRepo.List()
                                        .Where(c => ids.Contains(c.Id))
                                        .Select(c => c.Id)
                                        .ToHashSet();

            if (existentes.Count != ids.Count)
                throw new ArgumentException("Alguma cidade informada não existe.");

            var regiao = new Regiao(nome);

            foreach (var id in ids)
                regiao.RegiaoCidades.Add(new RegiaoCidade(regiao.Id, id));

            return _regiaoRepo.Add(regiao);
        }

        public Regiao Update(Guid id, string nome, IEnumerable<Guid> cidadeIds)
        {
            var regiao = _regiaoRepo.GetWithCidadesTracked(id);
            if (regiao == null) throw new ArgumentException("Região não encontrada.");

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var nomeNorm = nome.Trim();
                if (_regiaoRepo.Query(nomeNorm).Any(r => r.Id != id && r.Nome.Equals(nomeNorm, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("Já existe uma região com esse nome.");

                regiao.Nome = nomeNorm;
            }

            if (cidadeIds != null)
            {
                var ids = cidadeIds.Where(x => x != Guid.Empty).Distinct().ToList();
                if (!ids.Any())
                    throw new ArgumentException("Informe ao menos uma cidade.");

                var existentes = _cidadeRepo.List()
                                            .Where(c => ids.Contains(c.Id))
                                            .Select(c => c.Id)
                                            .ToHashSet();

                if (existentes.Count != ids.Count)
                    throw new ArgumentException("Alguma cidade informada não existe.");

                regiao.RegiaoCidades.Clear();
                foreach (var cid in ids)
                    regiao.RegiaoCidades.Add(new RegiaoCidade(regiao.Id, cid));
            }

            return _regiaoRepo.Update(regiao);
        }

        public void Delete(Guid id)
        {
            _regiaoRepo.Delete(id);
        }

        public Regiao Ativar(Guid id)
        {
            var regiao = _regiaoRepo.Get(id) ?? throw new ArgumentException("Região não encontrada.");
            if (!regiao.Ativo)
            {
                regiao.Ativo = true;
                _regiaoRepo.Update(regiao);
            }
            return regiao;
        }

        public Regiao Desativar(Guid id)
        {
            var regiao = _regiaoRepo.Get(id) ?? throw new ArgumentException("Região não encontrada.");
            if (regiao.Ativo)
            {
                regiao.Ativo = false;
                _regiaoRepo.Update(regiao);
            }
            return regiao;
        }

        public bool ExistsByName(string nome, Guid? ignoreId = null)
            => _regiaoRepo.ExistsByName(nome, ignoreId);
    }
}
