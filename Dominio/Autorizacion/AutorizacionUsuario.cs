using Dominio.Repos;

namespace Dominio.Autorizacion
{
    class AutorizacionUsuario : IAutorizacionUsuario
    {
        IUsuariosRepo UsuariosRepo;

        public AutorizacionUsuario(IUsuariosRepo usuariosRepo)
        {
            UsuariosRepo = usuariosRepo;
        }

        public bool AutorizarSobreReserva(int id, Reserva reserva)
        {
            var usuario = UsuariosRepo.getUsuario(id);

            if (usuario == null)
                return false;

            if(usuario.Tipo == TipoDeUsuario.Administrador)
                return true;

            if (usuario.Tipo == TipoDeUsuario.Miembro && reserva.Responsable.Id == id)
                return true;

            return false;
        }
    }
}
