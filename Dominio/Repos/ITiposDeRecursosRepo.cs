using System.Collections.Generic;
using Dominio.Entidades;

namespace Dominio.Repos
{
    public interface ITiposDeRecursosRepo
    {
        IList<TipoRecurso> Todos();
        TipoRecurso ObtenerPorId(int id);
        void Agregar(TipoRecurso entidad);
        void Actualizar(TipoRecurso entidad);
        void Eliminar(TipoRecurso entidad);
        bool ExisteId(int id);
    }
}