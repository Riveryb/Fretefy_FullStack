using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Entities
{
    public class Regiao : IEntity
    {
        public Regiao()
        {

        }

        public Regiao(string nome, List<Cidade> cidades)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            _cidades = cidades ?? new List<Cidade>();
        }

        public Guid Id { get; set; }

        public string Nome { get; set; }

        private readonly List<Cidade> _cidades = new List<Cidade>();

        public IReadOnlyCollection<Cidade> Cidades => _cidades.AsReadOnly();

        public void AdicionarCidade(Cidade cidade) => _cidades.Add(cidade);

        public void RemoverCidade(Cidade cidade) => _cidades.Remove(cidade);
    }
}