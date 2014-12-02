using System.Linq;
using Dominio;
using Dominio.Entidades;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class TiposDeCaracteristicasRepo : RepoBase<TipoCaracteristica>, ITiposDeCaracteristicasRepo
    {
        public TiposDeCaracteristicasRepo(ReservasContext ctx) : base(ctx)
        {
        }

        public TipoCaracteristica ObtenerPorNombre(string nombre)
        {
            return Ctx.TiposDeCaracteristicas
                .FirstOrDefault(tipoDeCaracteristica => tipoDeCaracteristica.Nombre == nombre);
        }
    }
}
