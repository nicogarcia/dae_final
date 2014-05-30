using Dominio;
using System;
using System.Collections.Generic;
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

        public DbSet<Usuario> Usuarios { get; set; }
    }

}
