using System;
using System.Collections.Generic;
using Dominio.Repos;

namespace Dominio
{
    public class ValidadorDeReserva
    {
        IReservaRepo ReservasRepo;
        IUsuariosRepo UsuariosRepo;
        IRecursosRepo RecursosRepo;

        public IDictionary<string, string> Errores { get; private set; }

        public ValidadorDeReserva (IReservaRepo reservaRepo, IUsuariosRepo usuariosRepo, IRecursosRepo recursosRepo)
        {
            ReservasRepo = reservaRepo;
            UsuariosRepo = usuariosRepo;
            RecursosRepo = recursosRepo;
        }

        public bool Validar(string usuario_responsable, string codigo_recurso, DateTime inicio, DateTime fin)
        {
            bool valido = true;
            Errores = new Dictionary<string, string>();
            
            if (inicio > fin)
            {
                Errores.Add("FechasIF", "La fecha de inicio es mayor que la fecha del fin");
                valido = false;
            }
                
            if (!UsuariosRepo.ExisteNombreUsuario(usuario_responsable))
            {
                Errores.Add("Responsable", "El usuario responsable ingresado no existe");
                valido = false;
            }

            if (!RecursosRepo.ExisteCodigo(codigo_recurso))
            {
                Errores.Add("RecursoReservado", "El codigo de recurso ingresado no existe");
                valido = false;
            }
            else if (ReservasRepo.ExisteReserva(codigo_recurso, inicio, fin))
            {
                Errores.Add("ReservaReservado",
                    "El recurso que desea reservar, ya que se encuentra reservado en esos horarios");
                valido = false;
            }

            return valido;
        }
    }
}
