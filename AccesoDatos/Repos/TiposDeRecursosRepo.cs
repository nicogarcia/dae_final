using Dominio;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class TiposDeRecursosRepo : RepoBase<TipoRecurso>, ITiposDeRecursosRepo
    {
        public TiposDeRecursosRepo(ReservasContext ctx) : base(ctx)
        {
        }
    }
}
