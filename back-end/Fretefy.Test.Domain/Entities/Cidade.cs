using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Entities
{
    public class Cidade : IEntity
    {
        public Cidade()
        {
            RegiaoCidades = new List<RegiaoCidade>();
        }

        public Cidade(string nome, string uf) : this()
        {
            Id = Guid.NewGuid();
            Nome = nome;
            UF = uf;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public ICollection<RegiaoCidade> RegiaoCidades { get; set; }
    }
}
