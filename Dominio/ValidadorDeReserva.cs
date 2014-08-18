using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Repos;

namespace Dominio
{
    public class ValidadorDeReserva
    {
          public IDictionary<string, string> Errores { get; private set; }
        IReservaRepo Reservarepo;

        public ValidadorDeReserva (IReservaRepo reservarepo)
        {
            this.Reservarepo = reservarepo;
        }


        public bool Validar(string usuario_responsable, string codigo_recurso, DateTime inicio, DateTime fin)
        {
            bool valido = true;
            Errores = new Dictionary<string, string>();
            if (inicio>fin)
            {
                Errores.Add("FechasIF", "La fecha de inicio es mayor que la fecha del fin");
                valido = false;
            }
                
            if (!Reservarepo.ExisteResponsable(usuario_responsable))
            {
                Errores.Add("Responsable", "El usuario responsable ingresado no existe");
                valido = false;
            }

            if (!Reservarepo.ExisteRecurso(codigo_recurso))
            {
                Errores.Add("RecursoReservado", "El codigo de recurso ingresado no existe");
                valido = false;
            }
            else
            {
                if (!Reservarepo.ExisteReserva(codigo_recurso,inicio,fin))
                {
                    Errores.Add("ReservaReservado", "El recurso que desea reserva, ya que se encuentra reservado en esos horarios");
                    valido = false;
                }

            }
            return valido;
        }
    }
}
