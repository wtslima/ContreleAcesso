using ControleAcesso.Dominio.Aplicacao.Interfaces;
using System.Web.Mvc;

namespace CONTROLE_DE_ACESSO_WEB.Controllers
{
    public abstract class BaseController<T> : Controller where T : class
    {
        public IServicoApp<T> ServicoAplicacao { get; set; }

        protected BaseController(IServicoApp<T> servicoAplicacao)
        {
            ServicoAplicacao = servicoAplicacao;
        }
    }
}