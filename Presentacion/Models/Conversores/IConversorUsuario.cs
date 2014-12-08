using Dominio.Entidades;

namespace Presentacion.Models.Conversores
{
    public interface IConversorUsuario
    {
        UsuarioVM CrearViewModel(Usuario u);

        Usuario ActualizarUsuario(UsuarioVM usuarioVM);

        Usuario CrearUsuario(UsuarioVM usuarioVM);
    }
}