using Dominio.Entidades;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class TiposDeRecursosRepo : RepoBase<TipoRecurso>, ITiposDeRecursosRepo
    {
        public TiposDeRecursosRepo(IReservasContext ctx) : base(ctx)
        {
        }

        public bool ExisteId(int id)
        {
            return ObtenerPorId(id) != null;
        }
    }
}
