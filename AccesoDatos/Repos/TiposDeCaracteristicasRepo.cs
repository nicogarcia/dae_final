using Dominio;

namespace AccesoDatos.Repos
{
    public class TiposDeCaracteristicasRepo : RepoBase<TipoCaracteristica>
    {
        public TiposDeCaracteristicasRepo(ReservasContext ctx) : base(ctx)
        {
        }
    }
}
