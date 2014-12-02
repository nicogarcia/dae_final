using Dominio;
using Dominio.Entidades;

namespace Presentacion.Models.Conversores
{
    public class ConversorReservaMV
    {
        public static ReservaVM convertirReserva(Reserva reserva)
        {
            ReservaVM reservaVM = new ReservaVM();

            reservaVM.Id = reserva.Id;
            reservaVM.Creador = reserva.Creador.NombreUsuario;
            reservaVM.Estado = reserva.Estado.ToString();
            reservaVM.FechaCreacion = reserva.FechaCreacion;
            reservaVM.Fin = reserva.Fin;
            reservaVM.Inicio = reserva.Inicio;
            
            reservaVM.RecursoReservado = reserva.RecursoReservado.Codigo;
            reservaVM.Responsable = reserva.Responsable.NombreUsuario;
            reservaVM.Descripcion = reserva.Descripcion;

            return reservaVM;
        }
        public static Reserva convertirReserva(ReservaVM reservaVM)
        {
            Reserva reserva = new Reserva();

            reserva.FechaCreacion = reservaVM.FechaCreacion;
            reserva.Fin = reservaVM.Fin;
            reserva.Inicio = reservaVM.Inicio;
            reserva.Descripcion = reservaVM.Descripcion;

            return reserva;
        }
    }
}