using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Entities
{
    public class Regiao : IEntity
    {
        public Regiao()
        {
            RegiaoCidades = new List<RegiaoCidade>(); Ativo = true;
        }

        public Regiao(string nome) : this()
        {
            Id = Guid.NewGuid(); Nome = nome;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public ICollection<RegiaoCidade> RegiaoCidades { get; set; }
    }
}
