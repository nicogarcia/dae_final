using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    /// <summary>
    /// Inicializador de EF que permite setear datos por defecto.
    /// </summary>
    public class ReservasDbInitializer: DropCreateDatabaseIfModelChanges<ReservasContext>
    {
        protected override void Seed(ReservasContext context)
        {
            base.Seed(context);
            
            // Add data

        }
    }
}
