using System.Data.Entity;
using Dominio;
using Dominio.Entidades;

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
            
            // Add admin user
            AddAdminUser(context);

            // Add data
            InitializeResources(context);            
        }

        private static void AddAdminUser(ReservasContext context)
        {
            var adminUser = new Usuario("admin", "admin", "admin", "33333", "", "admin@admin.com", "",
                TipoDeUsuario.Administrador);

            context.Usuarios.Add(adminUser);

            context.SaveChanges();
        }

        private static void InitializeResources(ReservasContext context)
        {
            var tipos = TiposPredefinidos.PoblarTiposPredefinidos();

            foreach (var tipo in tipos)
            {
                context.TiposDeRecursos.Add(tipo);
                foreach (var caracteristica in tipo.TiposDeCaracteristicas)
                    context.TiposDeCaracteristicas.Add(caracteristica);
            }

            context.SaveChanges();
        }
    }
}
