using Dominio;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class TiposDeCaracteristicasRepo : RepoBase<TipoCaracteristica>, ITiposDeCaracteristicasRepo
    {
        public TiposDeCaracteristicasRepo(ReservasContext ctx) : base(ctx)
        {
        }
    }
}
