using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Web;
using AutoMapper;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcessoService.DataContracts;
using log4net;
using Xipton.Razor;
using Entidades = ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Exceptions;
using ControleAcessoService.Elemento;

namespace ControleAcessoService
{
    [ServiceContract(Namespace = "http://inmetro.gov.br/ControleAcesso")]
    public interface IAutenticacaoServico
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "TodosUsuarios?token={token}")]
        [Description("Retorna lista de todos os usuários associados a um determinado sistema.")]
        List<Usuario> TodosUsuarios(string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AutenticarUsuario")]
        [Description("Autentica um usuário (registrado na rede interna do INMETRO) em um determinado sistema.")]
        Usuario AutenticarUsuario(string token, Login login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AutenticarUsuarioAnonimo")]
        [Description("Representa a autenticação para usuários anônimos.")]
        Usuario AutenticarUsuarioAnonimo(string token, string credencial);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AutenticarUsuarioExterno")]
        [Description("Autentica um usuário (de fora da rede interna do INMETRO) em um determinado sistema.")]
        Usuario AutenticarUsuarioExterno(string token, Login login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "SolicitarSenhaTemporaria")]
        [Description("Gera uma senha de acesso temporária para um determinado usuário externo.")]
        void SolicitarSenhaTemporaria(string token, string login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AlterarSenhaUsuarioExterno")]
        [Description("Modifica a senha de acesso de um determinado usuário externo.")]
        void AlterarSenhaUsuarioExterno(string token, string login, string senhaAtual, string novaSenha);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "PerfisSistema?token={token}")]
        [Description("Retorna lista de todos os perfis associados a um determinado sistema.")]
        List<String> PerfisSistema(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "ObterListaPerfisUsuarioExterno?token={token}&login={login}")]
        [Description("Retorna lista de todos os perfis associados a um determinado usuário.")]
        List<String> ObterListaPerfisUsuarioExterno(string token, string login);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "VerificacaoPeriodicaServico")]
        [Description("Retorna informações específicas para o monitorador de sistemas.")]
        RetornoVerificacaoServico VerificacaoPeriodicaServico();

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "CriarUsuario")]
        [Description("Cria uma conta de usuário.")]
        void CriarUsuarioExterno(string token, Usuario usuario);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AutenticacaoServico : IAutenticacaoServico
    {
        private string UserHostAddress { get { return HttpContext.Current.Request.UserHostAddress; } }
        private string UserHostName { get { return Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName; } }

        public ILog Log { get { return log4net.LogManager.GetLogger("CONTROLEACESSO"); } }

        public AutenticacaoServico()
        {
            Mapper.CreateMap<Entidades.Usuario, Usuario>();
            Mapper.CreateMap<Entidades.UsuarioExterno, Usuario>()
                .ForMember(dst => dst.Senha, src => src.MapFrom(p => p.Senha.Valor));
            Mapper.CreateMap<Entidades.SistemaPerfil, Perfil>();
            Mapper.CreateMap<Entidades.UsuarioSistemaPerfil, Perfil>();
        }

        public List<Usuario> TodosUsuarios(string token)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    return UsuarioServico.Instancia.Buscar().Select(u => Mapper.Map<Entidades.Usuario, Usuario>(u)).ToList();
                }

                Log.Warn(string.Format("Tentativa de acessar o método 'TodosUsuarios' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                throw new Exception("Token inválido.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void CriarUsuarioExterno(string token, Usuario usuario)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);

                    var usuarioEx = new Entidades.UsuarioExterno(false);
                    usuarioEx.Email = usuario.Email;
                    usuarioEx.IdPessoaFisica = usuario.PessoaFisica.Id;
                    usuarioEx.Login = usuario.Email;
                    usuarioEx.Nome = usuario.Nome;
                    

                    //var usuarioEx = new Entidades.UsuarioExterno
                    //{
                    //    Email = usuario.Email,
                    //    IdPessoaFisica = usuario.PessoaId,
                    //    Login = usuario.Email,
                    //    Nome = usuario.Nome
                    //};
                    var perfis = usuario.Perfis.Select(p => new ControleAcesso.Dominio.Entidades.SistemaPerfil { CodigoSistema = origem.CodigoSistema, CodigoPerfil = p.CodigoPerfil });

                    foreach (var item in perfis)
                        usuarioEx.AdicionarPerfil(item);

                    usuarioEx.AdicionarSenha(new Entidades.UsuarioExternoSenha(usuario.Email, usuario.Senha, DateTime.MaxValue));

                    var usuarioServico = new UsuarioExternoServico();
                    usuarioServico.SalvarComTransacao(usuarioEx);

                    usuarioServico.DesativarUsuario(usuario.Email);

                    //string senhaTemporaria = usuarioServico.SolicitarSenhaTemporaria(usuarioEx.Login, DateTime.Now.AddDays(-1));

                    //EnviarEmail(
                    //    "servir@inmetro.gov.br",
                    //    usuario.Email,
                    //    "[" + origem.CodigoSistema + "] Nova senha",
                    //    "~/EmailTemplates/" + origem.CodigoSistema + "SenhaTemporaria.cshtml",
                    //    new
                    //    {
                    //        Senha = senhaTemporaria,
                    //        Prazo = ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"]
                    //    });
                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'CriarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }


        }

        public Usuario AutenticarUsuario(string token, Login login)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    try
                    {
                        var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                        var usuario = UsuarioServico.Instancia.AutenticarUsuario(origem.CodigoSistema, login.UserName, login.Senha);
                        return Mapper.Map<Entidades.Usuario, Usuario>(usuario);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        throw ex;
                    }
                }

                Log.Warn(string.Format("Tentativa de acessar o método 'AutenticarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                throw new Exception("Token inválido.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }


        public Usuario AutenticarUsuarioAnonimo(string token, string credencial)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                //if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                //{
                try
                {

                    if (credencial == ConfigurationManager.AppSettings["CredencialAcessoAnonimo"])
                    {
                        //var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                        //var usuario = UsuarioServico.Instancia.AutenticarUsuario(origem.CodigoSistema, login.UserName, login.Senha);

                        var usuario = new Entidades.Usuario { Login = "Consumidor@Servir" };


                        return Mapper.Map<Entidades.Usuario, Usuario>(usuario);
                    }
                    else
                    {
                        throw new NaoAutorizadoException();
                    }


                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    throw ex;
                }
                //}

                Log.Warn(string.Format("Tentativa de acessar o método 'AutenticarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                throw new Exception("Token inválido.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }


        public List<String> PerfisSistema(string token)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                var objToken = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                if (token != null)
                {
                    var perfis = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals(objToken.CodigoSistema)).FirstOrDefault().PerfisAcesso;
                    return perfis.Where(p => p.Ativo).Select(p => p.Codigo.Trim()).ToList();
                }

                return new List<String>();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        #region Usuário externo
        public Usuario AutenticarUsuarioExterno(string token, Login login)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                    var usuario = UsuarioExternoServico.Instancia.Autenticar(origem.CodigoSistema, login.UserName, login.Senha);
                    return Mapper.Map<Entidades.UsuarioExterno, Usuario>(usuario);
                }

                Log.Warn(string.Format("Tentativa de acessar o método 'AutenticarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                throw new Exception("Token inválido.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void AlterarSenhaUsuarioExterno(string token, string login, string senhaAtual, string novaSenha)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    UsuarioExternoServico.Instancia.AlterarSenha(login, senhaAtual, novaSenha);
                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'SolicitarSenhaTemporaria' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void SolicitarSenhaTemporaria(string token, string login)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var sistema = SistemaServico.Instancia.BuscarPorToken(token).Codigo;
                    var senha = UsuarioExternoServico.Instancia.SolicitarSenhaTemporaria(login);
                    EnviarEmail(
                        "servir@inmetro.gov.br",
                        login,
                        "[" + sistema + "] Nova senha",
                        "~/EmailTemplates/" + sistema + "SenhaTemporaria.cshtml",
                        new
                        {
                            Senha = senha,
                            Prazo = ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"]
                        });
                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'SolicitarSenhaTemporaria' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public List<String> ObterListaPerfisUsuarioExterno(string token, string login)
        {
            try
            {

                if (login != "Consumidor@Servir")
                {
                    if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                    {
                        var servico = UsuarioExternoServico.Instancia;
                        var usuario = servico.Buscar(u => u.Login.ToLower().Equals(login.ToLower().Trim())).FirstOrDefault();

                        return usuario.Perfis.Select(p => p.CodigoPerfil.Trim()).ToList();
                    }
                }
                else
                {
                    List<string> lista = new List<string>();
                    lista.Add("Autentic");

                    return lista.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }

            return null;
        }
        #endregion

        private void EnviarEmail(string de, string para, string assunto, string urlTemplate, object modelo)
        {
            var rm = new RazorMachine();
            var template = rm.ExecuteUrl(urlTemplate, modelo);
            var corpo = template.Result;

            MailMessage mail = new MailMessage(de, para);
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "webmail.inmetro.gov.br";
            //client.Credentials = new System.Net.NetworkCredential("servir@inmetro.gov.br", "senha");
            mail.Subject = assunto;
            mail.IsBodyHtml = true;
            mail.Body = corpo;
            client.Send(mail);
        }


        #region Métodos Internos

        /// <summary>
        /// Método usado pelo monitorador de sistemas para verificar a situação do serviço. 
        /// </summary>
        /// <returns></returns>
        public RetornoVerificacaoServico VerificacaoPeriodicaServico()
        {
            // TODO: O ideal é colocar a montagem deste retorno na biblioteca corporativo, onde os demais sistemas a reutilizarão. VerificacaoPeriodicaServico deve
            // implementar uma interface de verificação periodica. Ou o serviço deve herdar de uma classe do Corporativo, contendo esta rotina. (Rodrigo)

            RetornoVerificacaoServico retorno = new RetornoVerificacaoServico();
            retorno.Mensagem = "";

            // TODO: Deve ser colocado na biblioteca corporativo.
            System.ServiceModel.Configuration.ClientSection clientSection = (System.ServiceModel.Configuration.ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            System.Text.StringBuilder endpoints = new System.Text.StringBuilder();

            for (int i = 0; i < clientSection.Endpoints.Count; i++)
                endpoints.Append(clientSection.Endpoints[i].Address.OriginalString);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // TODO: Colocar essa rotina na biblioteca do Corporativo. 
            retorno.EHash = Hash(endpoints.ToString());
            retorno.ScHash = Hash(ConfigurationManager.ConnectionStrings[1].ConnectionString.ToString());

            try
            {
                retorno.Consulta = ((SistemaServico.Instancia.Buscar(x => x.Ativo == true).Count() > 0));
            }
            catch (Exception ex)
            {
                retorno.Mensagem = ex.Message;
                //Logger.Fatal(String.Format("Ocorreu um erro na verificação periódica do serviço. Mensagem de exceção: '{0}'.", ex.Message));
            }

            return retorno;
        }

        private static string Hash(string conexaoBanco)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte[] hash;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(conexaoBanco));
            }

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }



        #endregion
    }
}