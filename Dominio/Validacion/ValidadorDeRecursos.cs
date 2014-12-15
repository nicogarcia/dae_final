using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dominio.Entidades;
using Dominio.Repos;

namespace Dominio.Validacion
{
    public class ValidadorDeRecursos : IValidadorDeRecursos
    {
        IRecursosRepo RecursosRepo;
        ITiposDeRecursosRepo TiposDeRecursosRepo;

        IDictionary<string, string> Errores;

        public ValidadorDeRecursos(IRecursosRepo recursosRepo, ITiposDeRecursosRepo tiposDeRecursosRepo)
        {
            RecursosRepo = recursosRepo;
            TiposDeRecursosRepo = tiposDeRecursosRepo;
            Errores = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ObtenerErrores()
        {
            return new ReadOnlyDictionary<string, string>(Errores);
        } 

        public bool EsValido(Recurso recurso)
        {
            Errores.Clear();

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

            if (RecursosRepo.ExisteCodigoEnOtroRecurso(recurso.Codigo, recurso.Id))
                Errores.Add("Codigo", "El código de recurso ya existe.");

            if (RecursosRepo.ExisteNombreEnOtroRecurso(recurso.Codigo, recurso.Id))
                Errores.Add("Nombre", "El nombre de recurso ya existe.");

            if (!TiposDeRecursosRepo.ExisteId(recurso.Tipo.Id))
                Errores.Add("TipoId", "El tipo de recurso no existe.");

            return Errores.Count == 0;
        }
        
    }
}
