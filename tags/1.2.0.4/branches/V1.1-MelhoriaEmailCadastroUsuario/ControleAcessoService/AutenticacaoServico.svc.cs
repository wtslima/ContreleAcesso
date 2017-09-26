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
using ControleAcessoService.Configuration;
using ControleAcessoService.DataContracts;
using Corporativo.Dominio.Entidades;
using log4net;
using Xipton.Razor;
using Xipton.Razor.Core;
using Entidades = ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Exceptions;
using ControleAcessoService.Elemento;
using PessoaFisica = ControleAcessoService.DataContracts.PessoaFisica;


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
                   BodyStyle = WebMessageBodyStyle.Wrapped)]
        [Description("Retorna lista de todos os usuários externos e internos associados a um determinado sistema.")]
        [FaultContract(typeof(TokenInvalidoFault))]
        List<Usuario> ObterTodosUsuarios(string token, FiltroObterTodosUsuariosEnum filtro, bool filtrarPorSistema);
        
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

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AssociarUsuarioAoSistemaPerfil")]
        [Description("Associa um usuário a um sistema perfil.")]
        [FaultContract(typeof(UsuarioInexistenteFault))]
        [FaultContract(typeof(TokenInvalidoFault))]
        void AssociarUsuarioAoSistemaPerfil(string token, Usuario usuario);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "BuscarUsuarioExterno")]
        [Description("Cria uma conta de usuário.")]
        Usuario BuscarUsuarioExterno(string token, string login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AtivarDesativarUsuarioExterno")]
        [Description("Ativa ou desativa a conta de um usuário externo.")]
        void AtivarDesativarUsuarioExterno(string token, string login, bool ativo);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "AtivarDesativarUsuarioInterno")]
        [Description("Ativa ou desativa a conta de um usuário interno.")]
        void AtivarDesativarUsuarioInterno(string token, string login, bool ativo);


        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Wrapped,
                   UriTemplate = "ObterPerfisSistema?token={token}")]
        [Description("Retorna lista de todos os perfis associados a um determinado sistema.")]
        List<Perfil> ObterPerfisSistema(string token);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AutenticacaoServico : IAutenticacaoServico
    {
        private string UserHostAddress { get { return HttpContext.Current.Request.UserHostAddress; } }
        private string UserHostName { get { return Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName; } }

        public ILog Log { get { return log4net.LogManager.GetLogger("CONTROLEACESSO"); } }

        public AutenticacaoServico()
        {
            Mapper.CreateMap<Corporativo.Dominio.Entidades.PessoaFisica, PessoaFisica>()
                .ForMember(dst => dst.Nascimento, m => m.MapFrom(src => src.DataNascimento))
                .ForMember(dst => dst.Sexo, m => m.MapFrom(src => src.Sexo == SexoTipo.Feminino ? "F" : "M"));

            Mapper.CreateMap<Entidades.Usuario, Usuario>()
                .ForMember(dst => dst.Tipo, m => m.MapFrom(src => TipoUsuario.Interno))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => p.Ativo ? "A" : "D"));
            Mapper.CreateMap<Entidades.UsuarioExterno, Usuario>()
                .ForMember(dst => dst.Senha, src => src.MapFrom(p => p.Senha.Valor))
                .ForMember(dst => dst.Tipo, m => m.MapFrom(src => TipoUsuario.Externo))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => p.Ativo ? "A" : "D"));
            Mapper.CreateMap<Entidades.SistemaPerfil, Perfil>()
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.Perfil.Nome));
            Mapper.CreateMap<Entidades.UsuarioSistemaPerfil, Perfil>()
                .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.CodigoPerfil))
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Nome));
            Mapper.CreateMap<Entidades.UsuarioExternoSistemaPerfil, Perfil>()
                .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.CodigoPerfil))
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Nome));
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

        public List<Usuario> ObterTodosUsuarios(string token, FiltroObterTodosUsuariosEnum filtro, bool filtrarPorSistema)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                    var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals(origem.CodigoSistema)).FirstOrDefault();

                    var buscarExternos = filtro == FiltroObterTodosUsuariosEnum.Todos || filtro == FiltroObterTodosUsuariosEnum.Externos;
                    var buscarInternos = filtro == FiltroObterTodosUsuariosEnum.Todos || filtro == FiltroObterTodosUsuariosEnum.Internos;
                    var usuarios = default(List<Usuario>);

                    System.Linq.Expressions.Expression<Func<Entidades.Usuario, bool>> criterio = null;
                    System.Linq.Expressions.Expression<Func<Entidades.UsuarioExterno, bool>> criterioExterno = null;
                    if (filtrarPorSistema)
                    {
                        criterio = i => i.Perfis.Any(p=> p.CodigoSistema == sistema.Codigo);
                        criterioExterno = i => i.Perfis.Any(p=> p.CodigoSistema == sistema.Codigo);
                    }

                    if(buscarInternos){
                        usuarios = UsuarioServico.Instancia.Buscar(criterio).Select(Mapper.Map<Usuario>).ToList();
                        if(buscarExternos)
                            usuarios= usuarios.Union(UsuarioExternoServico.Instancia.Buscar(criterioExterno).Select(Mapper.Map<Usuario>)).ToList();
                    } else {
                        usuarios = UsuarioExternoServico.Instancia.Buscar(criterioExterno).Select(Mapper.Map<Usuario>).ToList();
                    }

                    return usuarios;
                } else {
                    var message = string.Format("Tentativa de acessar o método 'ObterTodosUsuarios' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName);
                    Log.Warn(message);
                    var fault = new TokenInvalidoFault() { Token = token, Message= message};
                    throw new FaultException<TokenInvalidoFault>(fault);
                }

            } catch (TokenInvalidoException ex){

                Log.Error(ex.Message, ex);
                var fault = new TokenInvalidoFault() { Token = token, Message =ex.Message};
                throw new FaultException<TokenInvalidoFault>(fault);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        private IEnumerable<Perfil> CreatePerfis(Entidades.Usuario usuario, IEnumerable<Entidades.Perfil> todosPerfis){
            var list = new List<Perfil>();
            foreach (var item in usuario.Perfis){
                var perfil = todosPerfis.FirstOrDefault(i=> i.Codigo == item.CodigoPerfil);
                if(perfil != null)
                    list.Add(new Perfil(){ CodigoPerfil=item.CodigoPerfil, NomePerfil=perfil.Nome, CodigoSistema=item.CodigoSistema });
            }
            return list;
        }

        public void CriarUsuarioExterno(string token, Usuario usuario)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                    var usuarioEx = new Entidades.UsuarioExterno
                    {
                        Email = usuario.Email,
                        IdPessoaFisica = usuario.PessoaFisica.Id,
                        Login = usuario.Email,
                        Nome = usuario.Nome,
                        Ativo = true,
                        
                    };
                    var perfis = usuario.Perfis.Select(p => new ControleAcesso.Dominio.Entidades.SistemaPerfil { CodigoSistema = origem.CodigoSistema, CodigoPerfil = p.CodigoPerfil });

                    foreach (var item in perfis){
                        if(!usuarioEx.Perfis.Any(i=> i.CodigoPerfil != null && i.CodigoPerfil.Trim() == item.CodigoPerfil.Trim()))
                            usuarioEx.AdicionarPerfil(item);
                    }

                    var usuarioServico = new UsuarioExternoServico();
                    usuarioServico.Salvar(usuarioEx);

                    string senhaTemporaria = usuarioServico.SolicitarSenhaTemporaria(usuarioEx.Login, DateTime.Now.AddDays(AppSettings.PrazoExpiracaoSenhaTemporaria)); // -1));

                    EnviarEmail(
                        AppSettings.EmailAdministrador,
                        usuario.Email,
                        "[" + origem.CodigoSistema + "] Cadastro efetuado com sucesso",
                        "~/EmailTemplates/" + origem.CodigoSistema + "Cadastro.cshtml",
						null);
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

        public Usuario BuscarUsuarioExterno(string token, string login)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var usuario = UsuarioExternoServico.Instancia.Buscar(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();

                    return Mapper.Map<Entidades.UsuarioExterno, Usuario>(usuario);
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

        public List<Perfil> ObterPerfisSistema(string token)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                var objToken = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                if (token != null)
                {
                    var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals(objToken.CodigoSistema)).FirstOrDefault();
                    var perfis = sistema.PerfisAcesso;
                    return perfis.Where(p => p.Ativo).Select(p => new Perfil(){  CodigoPerfil=p.Codigo.Trim(), CodigoSistema=sistema.Codigo.Trim(), NomePerfil=p.Nome.Trim() }).ToList();
                }

                return new List<Perfil>();
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
        	try
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
	            mail.Subject = assunto;
	            mail.IsBodyHtml = true;	            mail.Body = corpo;
	            client.Send(mail);
        	}
            catch (TemplateException ex)
            {
                if (!ex.Message.StartsWith("Template") && !ex.Message.Contains("not found"))
                {
                    throw;
                }
            }
        }

        public void AssociarUsuarioAoSistemaPerfil(string token, Usuario usuario)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                    var usuarioDatabase = UsuarioServico.Instancia.Buscar(i => i.Login == usuario.Login).FirstOrDefault();
                    if (usuarioDatabase == null)
                    {
                        var usuarioExterno = UsuarioExternoServico.Instancia.Buscar(i => i.Login == usuario.Login).FirstOrDefault();
                        if (usuarioExterno == null)
                        {
                            var message = string.Format("Usuário ({0}) informado não existe em nossos registros.", usuario.Login);
                            var fault = new UsuarioInexistenteFault { Login = usuario.Login, Message = message };
                            throw new FaultException<UsuarioInexistenteFault>(fault);
                        }

                        foreach (var item in usuario.Perfis)
                        {
                            if (!usuarioExterno.Perfis.Any(i => i.SistemaPerfil.Perfil != null && i.SistemaPerfil.Perfil.Codigo != null && i.SistemaPerfil.Perfil.Codigo.Trim() == item.CodigoPerfil.Trim()))
                            {
                                var perfil = new ControleAcesso.Dominio.Entidades.SistemaPerfil()
                                {
                                    Ativo = true,
                                    Alteracao = DateTime.Now,
                                    CodigoSistema = origem.CodigoSistema,
                                    CodigoPerfil = item.CodigoPerfil,
                                    Uso = "I",
                                    Origem = "I",
                                };

                                perfil.UsuariosExternos = new List<Entidades.UsuarioExterno>();
                                perfil.UsuariosExternos.Add(usuarioExterno);
                                usuarioExterno.AdicionarPerfil(perfil);
                            }
                        }

                        UsuarioExternoServico.Instancia.Salvar(usuarioExterno);
                        return;
                    }

                    foreach (var item in usuario.Perfis)
                    {
                        var perfil = new ControleAcesso.Dominio.Entidades.SistemaPerfil()
                        {
                            Ativo = true,
                            Alteracao = DateTime.Now,
                            CodigoSistema = origem.CodigoSistema,
                            CodigoPerfil = item.CodigoPerfil,
                            Uso = "I",
                            Origem = "I",
                        };

                        usuarioDatabase.AdicionarPerfil(perfil);
                    }

                    UsuarioServico.Instancia.Salvar(usuarioDatabase);
                }
                else
                {
                    var message = string.Format("Tentativa de acessar o método 'AssociarUsuarioAoSistemaPerfil' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName);
                    Log.Warn(message);
                    var fault = new TokenInvalidoFault { Token = token, Message = message};
                    throw new FaultException<TokenInvalidoFault>(fault);
                }
            }
            catch (TokenInvalidoException ex)
            {
                Log.Error(ex.Message, ex);
                var fault = new TokenInvalidoFault { Token = token };
                throw new FaultException<TokenInvalidoFault>(fault);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
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

        public void AtivarDesativarUsuarioExterno(string token, string login, bool ativo)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                    var usuario = UsuarioExternoServico.Instancia.Buscar(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();
                    if (usuario != null)
                    {
                        usuario.AtivarDesativarTodosPerfis(origem.CodigoSistema, ativo);
                        UsuarioExternoServico.Instancia.Salvar(usuario);
                    }
                    else
                    {
                        throw new Exception("Usuário não encontrado.");
                    }
                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'AtivartDesativarUsuarioExterno' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void AtivarDesativarUsuarioInterno(string token, string login, bool ativo)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                var loginBusca = login.PadRight(20);

                if (SistemaServico.Instancia.ValidarToken(token, UserHostName))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token, UserHostName);
                    var usuario = UsuarioServico.Instancia.Buscar(x => x.Login.ToUpper().Equals(loginBusca.ToUpper())).FirstOrDefault();
                    if (usuario != null)
                    {
                        usuario.AtivarDesativarTodosPerfis(origem.CodigoSistema, ativo);
                        UsuarioServico.Instancia.Salvar(usuario);
                    }
                    else
                    {
                        throw new Exception("Usuário não encontrado.");
                    }
                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'AtivarDesativarUsuarioInterno' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }
    }
}