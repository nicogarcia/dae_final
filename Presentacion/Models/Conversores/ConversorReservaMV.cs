using Dominio;

namespace Presentacion.Models.Conversores
{
    public class ConversorReservaMV
    {
        public static ReservaVM convertirReserva(Reserva reserva)
        {
            ReservaVM reservaVM = new ReservaVM();

            reservaVM.Id = reserva.Id;
            reservaVM.Creador = ConversorUsuarioUsuarioVM.getInstance(reserva.Creador);
            reservaVM.Estado = reserva.Estado.ToString();
            reservaVM.FechaCreacion = reserva.FechaCreacion;
            reservaVM.Fin = reserva.Fin;
            reservaVM.Inicio = reserva.Inicio;

            RecursoVM recursoVM = new RecursoVM();
            recursoVM.Nombre = reserva.RecursoReservado.Nombre;
            recursoVM.Tipo = reserva.RecursoReservado.Tipo;

            reservaVM.RecursoReservado = recursoVM;
            reservaVM.Responsable = ConversorUsuarioUsuarioVM.getInstance(reserva.Responsable);
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