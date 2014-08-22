using Dominio;

namespace Presentacion.Models.Conversores
{
    public interface IConversorRecurso
    {
        RecursoVM CrearViewModelVacio();

        RecursoVM CrearViewModel(Recurso recurso);
        
        Recurso CrearDomainModel(RecursoVM recursoVM);
        
        Recurso ActualizarDomainModel(RecursoVM recursoVM);
        
        void PoblarTiposDeRecursosSelectList(RecursoVM recursoVM);

        void PoblarTiposDeRecursosSelectList(BusquedaRecursoVM busquedaRecursoVM);

        void PoblarTiposDeRecursosSelectListConCampoVacio(BusquedaRecursoVM busquedaRecursoVM);
    }
}