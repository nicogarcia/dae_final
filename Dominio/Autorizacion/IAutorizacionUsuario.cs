namespace Dominio.Autorizacion
{
    public interface IAutorizacionUsuario
    {
        bool AutorizarSobreReserva(int id, Reserva reserva);
    }
}