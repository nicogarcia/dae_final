using System.Collections.Generic;

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

        IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre);

        IList<Recurso> Buscar(string nombre, string codigo, string tipo, List<string> caracteristicasTipo,
            List<string> caracteristicasValor);

        Recurso ObtenerPorCodigo(string codigoRecurso);
    }
}