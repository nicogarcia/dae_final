using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Entidades;

namespace Dominio.Repos
{
    public interface IReservasRepo
    {
        IList<Reserva> Todos();

        Reserva ObtenerPorId(int id);

        void Agregar(Reserva entidad);

        void Actualizar(Reserva entidad);

        void Eliminar(Reserva entidad);

        bool ExisteReserva(string codigoRecurso, DateTime inicio, DateTime fin);

        IQueryable<Reserva> AsQueryable();
    }
}
