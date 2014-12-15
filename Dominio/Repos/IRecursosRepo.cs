using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Dominio.Repos
{
    public interface IRecursosRepo
    {
        IList<Recurso> Todos();
        
        Recurso ObtenerPorId(int id);
        
        void Agregar(Recurso entidad);
        
        void Actualizar(Recurso entidad);
        
        void Eliminar(Recurso entidad);

        bool ExisteCodigo(string codigo);

        bool ExisteNombre(string nombre);

        bool ExisteCodigoEnOtroRecurso(string codigo, int id);

        bool ExisteNombreEnOtroRecurso(string nombre, int id);

        Recurso ObtenerPorCodigo(string codigoRecurso);

        IQueryable<Recurso> AsQueryable();
    }
}