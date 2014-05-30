using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ReservasContext : DbContext
    {
        public ReservasContext()
            : base("Name=ReservasDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ReservasContext>()); 
        }
    }
}
