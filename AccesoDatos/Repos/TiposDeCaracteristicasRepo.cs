using Dominio;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class TiposDeCaracteristicasRepo : RepoBase<TipoCaracteristica>, ITiposDeCaracteriscasRepo
    {
        public TiposDeCaracteristicasRepo(ReservasContext ctx) : base(ctx)
        {
        }
    }
}
