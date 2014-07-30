using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Repos
{
    public interface IReservaRepo
    {
        IList<Reserva> Todos();

        Reserva ObtenerPorId(int id);

        void Agregar(Reserva entidad);

        void Actualizar(Reserva entidad);

        void Eliminar(Reserva entidad);

        IList<Reserva> buscarReservas(string fecha_desde, string fecha_hasta, string tipo_recurso, string usuario_responsable, string estado_reserva);
    }
}
