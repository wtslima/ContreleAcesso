using CONTROLE_DE_ACESSO_WEB.Models;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace CONTROLE_DE_ACESSO_WEB.Controllers
{
    public class AccountController : BaseController<Usuario>
    {

        public IUsuarioServicoApp UsuarioServicoApp { get; set; }

        public ISistemaServicoApp SistemaServicoApp { get; set; } 

        public AccountController(IUsuarioServicoApp usuarioServicoApp, ISistemaServicoApp sistemaServico) : base(usuarioServicoApp)
        {
            UsuarioServicoApp = usuarioServicoApp;
            SistemaServicoApp = sistemaServico;
        }
        
        public ActionResult Login(string returnUrl)
        {
         
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

#if DEBUG
            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, Environment.UserName),
                    new Claim(ClaimTypes.Email, Environment.UserName + "-cast@inmetro.gov.br")
                },
                    "ApplicationCookie");

            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignIn(identity);
#endif


            ViewBag.ApiInfo = JsonConvert.SerializeObject(HttpContext.Application["VersionNumber"]);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            
            //string token = ConfigurationManager.AppSettings["token"];

            string codSistema = ConfigurationManager.AppSettings["codSistema"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                
                //var servico = new AutenticacaoServico();
                //var usuario = servico.AutenticarUsuario(token, new Login { UserName = model.Username, Senha = model.Password });
                var sistema = SistemaServicoApp.BuscarPorCodigoOuDescricao(codSistema).FirstOrDefault();

                var usuario = UsuarioServicoApp.AutenticarUsuario(sistema.Id, model.Username, model.Password);
                
                if (usuario.Nome.Trim().Contains(" "));
                    var nomeExibicao = usuario.Nome.Split(' ')[0];
                
                var identity = new ClaimsIdentity(new[]
		        {
		            new Claim(ClaimTypes.Name, nomeExibicao),
		            new Claim(ClaimTypes.Email, usuario.Email)
                    
                },
                    "ApplicationCookie");

                var context = Request.GetOwinContext();
                var authManager = context.Authentication;

                authManager.SignIn(identity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginError", "Login ou senha incorretos.");
                return View(model);
            }

            return Redirect(GetRedirectUrl(model.ReturnUrl));
        }
            
        private dynamic AuthenticateUser(LoginViewModel model)
        {
            var request = WebRequest.Create(ConfigurationManager.AppSettings["ControleAcessoUrl"] + "/AutenticacaoServico/REST/AutenticarUsuario");
            var postData = new
            {
                token = ConfigurationManager.AppSettings["ControleAcessoToken"],
                login = new
                {
                    UserName = model.Username,
                    Senha = model.Password
                }
            };
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData));
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            request.Method = "POST";
            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            var response = request.GetResponse();
            var status = ((HttpWebResponse)response).StatusCode;

            if (status != HttpStatusCode.OK) return null;

            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var user = new
            {
                AutenticarUsuarioResult = new
                {
                    Login = "",
                    Nome = "",
                    Email = "",
                    PessoaFisica = new { Id = 0 },
                    Perfis = new List<dynamic> {
                        new {
                            CodigoSistema = "",
                            CodigoPerfil = ""
                        }
                    }
                }
            };
            user = JsonConvert.DeserializeAnonymousType(reader.ReadToEnd(), user);

            var perfis = user.AutenticarUsuarioResult.Perfis.ToArray();
            if (perfis.All(p => p.CodigoPerfil.ToString().Trim().ToUpper() != "AUTENTIC")) return null;

            return user;
        }

        public ActionResult Logout()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignOut("ApplicationCookie");

            return RedirectToAction("Login", "Account");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }
    }
}