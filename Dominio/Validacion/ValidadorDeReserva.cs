using System;
using System.Collections.Generic;
using Dominio.Repos;

namespace Dominio.Validacion
{
    public class ValidadorDeReserva : IValidadorDeReserva
    {
        IReservasRepo ReservasRepo;
        IUsuariosRepo UsuariosRepo;
        IRecursosRepo RecursosRepo;

        public IDictionary<string, string> Errores { get; private set; }

        public ValidadorDeReserva (IReservasRepo reservaRepo, IUsuariosRepo usuariosRepo, IRecursosRepo recursosRepo)
        {
            ReservasRepo = reservaRepo;
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursosRepo;
        }

        public IDictionary<string, string> ObtenerErrores()
        {
            return Errores;
        } 

        public bool Validar(string usuarioResponsable, string codigoRecurso, DateTime inicio, DateTime fin)
        {
            bool valido = true;
            Errores = new Dictionary<string, string>();
            
            if (inicio > fin)
            {
                Errores.Add("Inicio", "La fecha de inicio es mayor que la fecha del fin");
                valido = false;
            }
                
            if (!UsuariosRepo.ExisteNombreUsuario(usuarioResponsable))
            {
                Errores.Add("Responsable", "El usuario responsable ingresado no existe");
                valido = false;
            }

            if (UsuariosRepo.BuscarUsuario(usuarioResponsable).IsLocked())
            {
                Errores.Add("Responsable", "El usuario responsable no puede realizar reservas");
                valido = false;
            }

            if (!RecursosRepo.ExisteCodigo(codigoRecurso))
            {
                Errores.Add("RecursoReservado", "El codigo de recurso ingresado no existe");
                valido = false;
            }
            else if (ReservasRepo.ExisteReserva(codigoRecurso, inicio, fin))
            {
                Errores.Add("RecursoReservado",
                    "El recurso que desea reservar, ya que se encuentra reservado en esos horarios");
                valido = false;
            }

            return valido;
        }
    }
}
