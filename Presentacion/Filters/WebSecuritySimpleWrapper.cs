using WebMatrix.WebData;

namespace Presentacion.Filters
{
    public class WebSecuritySimpleWrapper : IWebSecuritySimpleWrapper
    {

        public string GetCurrentUsername()
        {
            return WebSecurity.CurrentUserName;
        }

        public int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }
    }
}