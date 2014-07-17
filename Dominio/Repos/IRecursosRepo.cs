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

        IList<Recurso> FiltrarYOrdenar(string orden, string filtroCodigo, string filtroTipo, string filtroNombre);

        bool ExisteCodigo(string codigo);

        bool ExisteNombre(string nombre);
    }
}