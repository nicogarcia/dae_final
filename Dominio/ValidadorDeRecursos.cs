using System.Linq;
using Dominio.Repos;
using System.Collections.Generic;


namespace Dominio
{
    public class ValidadorDeRecursos
    {
        IRecursosRepo RecursosRepo;
        ITiposDeRecursosRepo TiposDeRecursosRepo;

        public IDictionary<string, string> Errores { get; private set; }

        public ValidadorDeRecursos(IRecursosRepo recursosRepo, ITiposDeRecursosRepo tiposDeRecursosRepo)
        {
            RecursosRepo = recursosRepo;
            TiposDeRecursosRepo = tiposDeRecursosRepo;
        }

        public bool EsValido(Recurso recurso)
        {
            Errores = new Dictionary<string, string>();

            if (RecursosRepo.ExisteCodigo(recurso.Codigo))
                Errores.Add("Codigo", "El código de recurso ya existe.");

            if (RecursosRepo.ExisteNombre(recurso.Nombre))
                Errores.Add("Nombre", "El nombre de recurso ya existe.");

            if (!TiposDeRecursosRepo.ExisteId(recurso.Tipo.Id))
                Errores.Add("TipoId", "El tipo de recurso no existe.");

            return Errores.Count == 0;
        }

        public bool EsValidoParaActualizar(Recurso recurso)
        {
            Errores = new Dictionary<string, string>();

            if (RecursosRepo.Todos().Any(rec => (rec.Codigo == recurso.Codigo) && (rec.Id != recurso.Id)))
                Errores.Add("Codigo", "El código de recurso ya existe.");

            if (RecursosRepo.Todos().Any(rec => (rec.Nombre == recurso.Nombre) && (rec.Id != recurso.Id)))
                Errores.Add("Nombre", "El nombre de recurso ya existe.");

            if (!TiposDeRecursosRepo.ExisteId(recurso.Tipo.Id))
                Errores.Add("TipoId", "El tipo de recurso no existe.");

            return Errores.Count == 0;
        }
        
    }
}
