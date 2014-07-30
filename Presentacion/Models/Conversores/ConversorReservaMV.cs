using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentacion.Models.Conversores
{
    public class ConversorReservaMV
    {
        public static ReservaVM convertirReserva(Reserva r)
        {
            ReservaVM toR = new ReservaVM();
            toR.Creador = ConversorUsuarioUsuarioVM.getInstance(r.Creador);
            toR.Estado = r.Estado.ToString();
            toR.FechaCreacion = r.FechaCreacion;
            toR.Fin = r.Fin;
            toR.Inicio = r.Inicio;
            RecursoVM rec = new RecursoVM();
            rec.Nombre = r.RecursoReservado.Nombre;
            rec.Tipo = r.RecursoReservado.Tipo;
            toR.RecursoReservado = rec;
            toR.Responsable = ConversorUsuarioUsuarioVM.getInstance(r.Responsable);
            toR.Descripcion = r.Descripcion;

            return toR;
        }
        public static Reserva convertirReserva(ReservaVM r)
        {
            Reserva toR = new Reserva();
            toR.FechaCreacion = r.FechaCreacion;
            toR.Fin = r.Fin;
            toR.Inicio = r.Inicio;
            toR.Descripcion = r.Descripcion;

            return toR;
        }
    }
}