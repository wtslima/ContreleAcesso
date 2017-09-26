using System.Web;
using System.Web.Mvc;

namespace CONTROLE_DE_ACESSO_WEB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
