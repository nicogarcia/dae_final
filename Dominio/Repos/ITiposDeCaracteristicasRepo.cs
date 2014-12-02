using System.Collections.Generic;
using Dominio.Entidades;

namespace Dominio.Repos
{
    public interface ITiposDeCaracteristicasRepo
    {
        IList<TipoCaracteristica> Todos();
        
        TipoCaracteristica ObtenerPorId(int id);
        
        void Agregar(TipoCaracteristica entidad);
        
        void Actualizar(TipoCaracteristica entidad);
        
        void Eliminar(TipoCaracteristica entidad);

        TipoCaracteristica ObtenerPorNombre(string nombre);
    }
}