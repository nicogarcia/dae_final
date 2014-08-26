using System;
using System.Collections.Generic;

namespace Dominio.Repos
{
    public interface IReservaRepo
    {
        IList<Reserva> Todos();

        Reserva ObtenerPorId(int id);

        void Agregar(Reserva entidad);

        void Actualizar(Reserva entidad);

        void Eliminar(Reserva entidad);

        bool ExisteReserva(string codigo_recurso, DateTime inicio, DateTime fin);

        IList<Reserva> buscarReservas(string fecha_desde, string fecha_hasta, string tipo_recurso, string usuario_responsable, string estado_reserva);

        
    }
}
