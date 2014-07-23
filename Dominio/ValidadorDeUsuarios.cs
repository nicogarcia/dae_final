using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominio
{
    public class ValidadorDeUsuarios
    {
        public IDictionary<string, string> Errores { get; private set; }
        IUsuariosRepo usuariorepo;

        public ValidadorDeUsuarios (IUsuariosRepo usuariorepo)
        {
            this.usuariorepo = usuariorepo;
        }

        public bool Validar (Usuario usuario)
        {
            bool valido = true;
            Errores = new Dictionary<string, string>();
            
            if (usuariorepo.ExisteNombreUsuario(usuario.NombreUsuario))
            {
                Errores.Add("NombreUsuario", "Este nombre de usuario ya existe.");
                valido = false;
            }

            if (usuariorepo.ExisteEmail(usuario.Email))
            {
                Errores.Add("Email", "Este email ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            if (usuariorepo.ExisteDNI(usuario.DNI))
            {
                Errores.Add("DNI", "Este DNI ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            if (usuariorepo.ExisteLegajo(usuario.Legajo))
            {
                Errores.Add("Legajo", "Este Legajo ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            return valido;
        }
        public bool validarUsuario(Usuario u)
        {
            
            bool valido = true;
            if (usuariorepo.ChequearExistenciaEmail(u))
            {
                Errores.Add("Email", "Este email ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            if (usuariorepo.ChequearExistenciaDNI(u))
            {
                Errores.Add("DNI", "Este DNI ya se encuentra registrado en otro usuario.");
                valido = false;
            }

            if (usuariorepo.ChequearExistenciaLegajo(u))
            {
                Errores.Add("Legajo", "Este Legajo ya se encuentra registrado en otro usuario.");
                valido = false;
            }
            return valido;
        }
        
    }
}
