using Dominio;
using Dominio.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Repos
{
    class ReservaRepo : RepoBase<Reserva>, IReservaRepo
    {
        public ReservaRepo(ReservasContext reservasContext): base(reservasContext)
        {

        }


    }
}
