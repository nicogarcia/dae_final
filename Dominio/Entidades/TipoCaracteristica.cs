
namespace Dominio.Entidades
{
    public class TipoCaracteristica
    {
        public int Id { get; private set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public TipoCaracteristica() { }
        public TipoCaracteristica(string nombre)
        {
            Nombre = nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }

    }
}
