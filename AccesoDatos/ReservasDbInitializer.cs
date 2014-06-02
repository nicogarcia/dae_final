using System.Data.Entity;
using Dominio;

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
            InitializeResources(context);            
        }

        private static void InitializeResources(ReservasContext context)
        {
            var tipos = TiposPredefinidos.ObtenerTiposPredefinidos();

            foreach (var tipo in tipos)
                context.TiposDeRecursos.Add(tipo);

            context.SaveChanges();
        }
    }
}
