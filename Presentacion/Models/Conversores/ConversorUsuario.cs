using Dominio;

namespace Presentacion.Models.Conversores
{
    public class ConversorUsuario
    {
        public static UsuarioVM getInstance(Usuario u)
        {
            UsuarioVM toR = new UsuarioVM(u.Id);
            toR.Apellido = u.Apellido;
            toR.DNI = u.DNI;
            toR.Email = u.Email;
            toR.EstadoUsuario = u.EstadoUsuario.ToString();
            toR.Legajo = u.Legajo;
            toR.Nombre = u.Nombre;
            toR.NombreUsuario = u.NombreUsuario;
            toR.Telefono = u.Telefono;
            toR.Tipo = u.Tipo;
            return toR;
        }
        public static Usuario getInstance(Usuario toR, UsuarioVM u)
        {
            toR.Apellido = u.Apellido;
            toR.DNI = u.DNI;
            toR.Email = u.Email;
            //toR.EstadoUsuario = u.EstadoUsuario.ToString();
            //toR.Id = u.id;
            toR.Legajo = u.Legajo;
            toR.Nombre = u.Nombre;
            //toR.NombreUsuario = u.NombreUsuario;
            toR.Telefono = u.Telefono;
            toR.Tipo = u.Tipo;
            return toR;
        }
    }
}