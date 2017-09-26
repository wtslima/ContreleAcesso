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
using ControleAcessoService;
using ControleAcessoService.DataContracts;
using Newtonsoft.Json;
using ControleAcesso.Web.UI.Models;



namespace ControleAcesso.Web.UI.Controllers
{
	public class AccountController : Controller
	{
        

	    public AccountController()
	    {
	            
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
        public async Task<ActionResult> Login( LoginViewModel model)
        {
           

             string token = ConfigurationManager.AppSettings["token"];
            if (!ModelState.IsValid)
            {
                return View(model);
            }

		    try
		    {
		        var servico = new AutenticacaoServico();
		        var usuario = servico.AutenticarUsuario(token, new Login {UserName = model.Username, Senha = model.Password});
		        var identity = new ClaimsIdentity(new[]
		        {
		            new Claim(ClaimTypes.Name, usuario.Nome),
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
                login= new {
                    UserName= model.Username,
                    Senha= model.Password
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
            var user = new {
                AutenticarUsuarioResult = new {
                    Login = "",
                    Nome =  "",
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