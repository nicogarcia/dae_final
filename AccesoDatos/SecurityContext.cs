using Dominio.Security;
using System.Data.Entity;

namespace AccesoDatos
{
    public class SecurityContext : DbContext
    {
        public SecurityContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
