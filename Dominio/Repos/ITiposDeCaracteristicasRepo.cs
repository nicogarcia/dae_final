﻿using System.Collections.Generic;

namespace Dominio.Repos
{
    public interface ITiposDeCaracteriscasRepo
    {
        IList<TipoCaracteristica> Todos();
        TipoCaracteristica ObtenerPorId(int id);
        void Agregar(TipoCaracteristica entidad);
        void Actualizar(TipoCaracteristica entidad);
        void Eliminar(TipoCaracteristica entidad);
    }
}