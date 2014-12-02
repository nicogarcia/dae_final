namespace Dominio.Entidades
{
    public class Caracteristica
    {
        public int Id { get; private set; }
        public virtual TipoCaracteristica Tipo { get; set; }
        public string Valor { get; set; }

        public Caracteristica()
        {
        }

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
