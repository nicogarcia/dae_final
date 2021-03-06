﻿using Dominio.Entidades;

namespace Presentacion.Models.Conversores
{
    public interface IConversorReserva
    {
        ReservaVM ConvertirReserva(Reserva reserva);
        ReservaVM CrearReservaVM();
        ReservaVM CrearReservaVM(string codigoRecurso);
        void PoblarSelectUsuario(ReservaVM reservaVM);
        BusquedaReservasVM CrearBusquedaReservasVM();
        void PoblarSelectRecursos(BusquedaReservasVM busquedaReservasVM);
        void PoblarSelectUsuarios(BusquedaReservasVM busquedaReservasVM);
        void PoblarSelectEstadosDeReserva(BusquedaReservasVM busquedaReservasVM);
        Reserva EditarReserva(ReservaVM reservaVM, string currentUser);
        void PoblarSelectRecursos(ReservaVM reservaVM);
    }
}