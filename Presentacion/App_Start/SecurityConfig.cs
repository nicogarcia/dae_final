using AccesoDatos;
using Dominio;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using WebMatrix.WebData;


namespace Presentacion
{
    public static class SecurityConfig
    {
        private const string DefaultAdminUsername = "admin";
        private const string DefaultAdminPwd = "123";

        public static void RegisterAuth()
        {
            using (var context = new SecurityContext())
            {
                if (!context.Database.Exists())
                {
                    // Create the SimpleMembership database without Entity Framework migration schema
                    ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                }
                context.Database.Initialize(true);
            }

            if (!WebSecurity.Initialized)
            { 
                WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                     "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            CreateRoles();

            CreateDefaultUser();
        }

        private static void CreateRoles()
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (!roles.RoleExists(TipoDeUsuario.Administrador.ToString()))
            {
                roles.CreateRole(TipoDeUsuario.Administrador.ToString());
            }

            if (!roles.RoleExists(TipoDeUsuario.Miembro.ToString()))
            {
                roles.CreateRole(TipoDeUsuario.Miembro.ToString());
            }
        }

        private static void CreateDefaultUser()
        {
            var membership = (SimpleMembershipProvider)Membership.Provider;
            if (membership.GetUser(DefaultAdminUsername, false) == null)
            {
                membership.CreateUserAndAccount(DefaultAdminUsername, DefaultAdminPwd);
                var roles = (SimpleRoleProvider)Roles.Provider;
                roles.AddUsersToRoles(new[] { DefaultAdminUsername }, new[] { TipoDeUsuario.Administrador.ToString(), TipoDeUsuario.Miembro.ToString() });
            }
        }

    }
}
