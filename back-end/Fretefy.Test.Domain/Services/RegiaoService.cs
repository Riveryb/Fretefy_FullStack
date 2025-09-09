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
            var regiao = _regiaoRepo.Get(id);
            if (regiao == null) throw new ArgumentException("Região não encontrada.");
            return regiao;
        }

        public IEnumerable<Regiao> List() =>
            _regiaoRepo.List().ToList();

        public IEnumerable<Regiao> Query(string terms) =>
            _regiaoRepo.Query(terms);

        public Regiao Create(string nome, IEnumerable<Guid> cidadeIds)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome da região é obrigatório.");

            if (_regiaoRepo.ExistsByName(nome.Trim()))
                throw new ArgumentException("Já existe uma região com esse nome.");

            var cidades = ResolveCidades(cidadeIds);

            var regiao = new Regiao(nome.Trim(), cidades);
            _regiaoRepo.Add(regiao);
            return regiao;
        }

        public Regiao Update(Guid id, string nome, IEnumerable<Guid> cidadeIds)
        {
            var regiao = _regiaoRepo.Get(id);
            if (regiao == null) throw new ArgumentException("Região não encontrada.");

            if (!string.IsNullOrWhiteSpace(nome))
            {
                var novoNome = nome.Trim();
                if (!novoNome.Equals(regiao.Nome, StringComparison.OrdinalIgnoreCase)
                    && _regiaoRepo.ExistsByName(novoNome))
                    throw new ArgumentException("Já existe uma região com esse nome.");
                regiao.Nome = novoNome;
            }

            if (cidadeIds != null)
            {
                var cidades = ResolveCidades(cidadeIds);
                // recria a lista com encapsulamento via métodos
                // limpa
                foreach (var c in regiao.Cidades.ToList())
                    regiao.RemoverCidade(c);
                // adiciona
                foreach (var c in cidades)
                    regiao.AdicionarCidade(c);
            }

            _regiaoRepo.Update(regiao);
            return regiao;
        }

        public void Delete(Guid id)
        {
            // poderia validar regras, ex: impedir delete se vinculada a algo
            _regiaoRepo.Delete(id);
        }

        private List<Cidade> ResolveCidades(IEnumerable<Guid> ids)
        {
            var list = new List<Cidade>();
            if (ids == null) return list;

            var idSet = new HashSet<Guid>(ids);
            foreach (var id in idSet)
            {
                // Ideal: ter ICidadeRepository.Get(Guid)
                // fallback: _cidadeRepo.List().FirstOrDefault(x => x.Id == id)
                // Aqui vou assumir que existe Get(Guid)
                var c = _cidadeRepo.Get(id);
                if (c == null) throw new ArgumentException($"Cidade {id} não encontrada.");
                list.Add(c);
            }
            return list;
        }
    }
}
