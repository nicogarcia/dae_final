using System.Data.Entity;
using System.Linq;
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
            var tipos = TiposPredefinidos.PoblarTiposPredefinidos();

            foreach (var tipo in tipos)
            {
                context.TiposDeRecursos.Add(tipo);
                foreach (var caracteristica in tipo.TiposDeCaracteristicas)
                    context.TiposDeCaracteristicas.Add(caracteristica);
            }

            context.SaveChanges();
            context.Recursos.ToList();
        }
    }
}
