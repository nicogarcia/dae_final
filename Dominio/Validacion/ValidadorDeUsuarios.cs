using System.Collections.Generic;
using Dominio.Entidades;
using Dominio.Repos;

namespace Dominio.Validacion
{
    public class ValidadorDeUsuarios : IValidadorDeUsuarios
    {
        IUsuariosRepo UsuariosRepo;

        private IDictionary<string, string> Errores { get; set; }

        public ValidadorDeUsuarios(IUsuariosRepo usuariorepo)
        {
            UsuariosRepo = usuariorepo;
        }

        public bool Validar(string username, string email, string dni, string legajo, int usuarioId = -1)
        {
            bool valido = true;
            Errores = new Dictionary<string, string>();

            if (UsuariosRepo.ExisteNombreUsuario(username, usuarioId))
            {
                Errores.Add("NombreUsuario", "Este nombre de usuario ya existe.");
                valido = false;
            }

            if (UsuariosRepo.ExisteEmail(email, usuarioId))
            {
                Errores.Add("Email", "Este email ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            if (UsuariosRepo.ExisteDNI(dni, usuarioId))
            {
                Errores.Add("DNI", "Este DNI ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            if (UsuariosRepo.ExisteLegajo(legajo, usuarioId))
            {
                Errores.Add("Legajo", "Este Legajo ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            return valido;
        }

        public IDictionary<string, string> ObtenerErrores()
        {
            return Errores;
        } 

        
    }
}
