using System.Data.Entity;

namespace AccesoDatos
{
    /// <summary>
    /// Entity Framework DB Context
    /// </summary>
    public class ReservasContext : DbContext
    {
        public ReservasContext()
            : base("Name=ReservasDB")
        {
            Database.SetInitializer(new ReservasDbInitializer()); 
        }
    }

}
