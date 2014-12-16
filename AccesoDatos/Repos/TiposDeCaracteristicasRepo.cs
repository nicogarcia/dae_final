using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class TiposDeCaracteristicasRepo : RepoBase<TipoCaracteristica>, ITiposDeCaracteristicasRepo
    {
        public TiposDeCaracteristicasRepo(IReservasContext ctx) : base(ctx)
        {
        }

        public TipoCaracteristica ObtenerPorNombre(string nombre)
        {
            return Ctx.TiposDeCaracteristicas
                .FirstOrDefault(tipoDeCaracteristica => tipoDeCaracteristica.Nombre == nombre);
        }
    }
}
