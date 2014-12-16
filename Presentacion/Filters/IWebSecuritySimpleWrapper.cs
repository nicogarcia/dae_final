namespace Presentacion.Filters
{
    public interface IWebSecuritySimpleWrapper
    {
        string GetCurrentUsername();
        int GetCurrentUserId();
    }
}