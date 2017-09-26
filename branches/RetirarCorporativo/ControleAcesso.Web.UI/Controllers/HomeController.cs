using System.Web.Mvc;

namespace ControleAcesso.Web.UI.Controllers
{
    public class HomeController : Controller
    {
    	[Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}