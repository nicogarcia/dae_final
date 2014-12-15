using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace AccesoDatos.Repos
{
    public class RecursosRepo : RepoBase<Recurso>, IRecursosRepo
    {
        public RecursosRepo(IReservasContext ctx)
            : base(ctx)
        {

        }

        public bool ExisteCodigo(string codigo)
        {
            return Ctx.Recursos.Any(recurso => recurso.Codigo == codigo);
        }

        public bool ExisteNombre(string nombre)
        {
            return Ctx.Recursos.Any(recurso => recurso.Nombre == nombre);
        }

        public Recurso ObtenerPorCodigo(string codigo)
        {
            return Ctx.Recursos.FirstOrDefault(recurso => recurso.Codigo == codigo);
        }
        
    }
}