
using Dominio;

namespace AccesoDatos.Repos
{
    public class TiposDeRecursosRepo : RepoBase<TipoRecurso>
    {
        public TiposDeRecursosRepo(ReservasContext ctx) : base(ctx)
        {
        }
    }
}
