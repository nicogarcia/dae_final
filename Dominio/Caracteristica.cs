using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio
{
    public class Caracteristica
    {
        public int Id { get; set; }
        public TipoCaracteristica Tipo { get; set; }
        public string Valor { get; set; }

        public Caracteristica() { }
        public Caracteristica(TipoCaracteristica tipo, string valor)
        {
            Tipo = tipo;
            Valor = valor;
        }

        public override string ToString()
        {
            return Tipo.ToString() + " = " + Valor;
        }
    }
}
