using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Transactions;
using System.Web;
using AutoMapper;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Servicos;
using ControleAcesso.Dominio.Helpers;
using ControleAcesso.Dominio.ObjetosDeValor;
using ControleAcessoService.Configuration;
using ControleAcessoService.DataContracts;
using log4net;
using Xipton.Razor;
using Xipton.Razor.Core;
using Entidades = ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Exceptions;
using ControleAcessoService.Elemento;
using Corporativo.Exceptions;
using PessoaFisica = ControleAcessoService.DataContracts.PessoaFisica;
using Corporativo.Utils.ActiveDirectory;

namespace ControleAcessoService
{
    [ServiceContract(Namespace = "http://inmetro.gov.br/ControleAcesso")]
    public interface IAutenticacaoServico
    {
        //[AspNetCacheProfile("ListaUsuarios")]
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "TodosUsuarios?token={token}")]
        [Description("Retorna lista de todos os usuários associados a um determinado sistema.")]
        List<Usuario> TodosUsuarios(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "ObterUsuario?token={token}&login={login}")]
        [Description("Retorna um único usuário com um determinado login.")]
        Usuario ObterUsuario(string token, string login);

       
        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [Description("Retorna lista de todos os usuários externos e internos associados a um determinado sistema.")]
        [FaultContract(typeof(TokenInvalidoFault))]
        List<Usuario> ObterTodosUsuarios(string token, FiltroObterTodosUsuariosEnum filtro, bool filtrarPorSistema);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AutenticarUsuario")]
        [Description("Autentica um usuário (registrado na rede interna do INMETRO) em um determinado sistema.")]
        Usuario AutenticarUsuario(string token, Login login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AutenticarUsuarioAnonimo")]
        [Description("Representa a autenticação para usuários anônimos.")]
        Usuario AutenticarUsuarioAnonimo(string token, string credencial);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AutenticarUsuarioExterno")]
        [Description("Autentica um usuário (de fora da rede interna do INMETRO) em um determinado sistema.")]
        Usuario AutenticarUsuarioExterno(string token, Login login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "Autenticar")]
        [Description("Autentica um usuário em um determinado sistema.")]
        Usuario Autenticar(string token, Login login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "SolicitarSenhaTemporaria")]
        [Description("Gera uma senha de acesso temporária para um determinado usuário externo.")]
        void SolicitarSenhaTemporaria(string token, string login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AtualizarPerfis")]
        [Description("Atualiza a lista de perfis de um determinado usuário.")]
        void AtualizarPerfis(string token, string login, List<string> perfis);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AlterarSenhaUsuarioExterno")]
        [Description("Modifica a senha de acesso de um determinado usuário externo.")]
        void AlterarSenhaUsuarioExterno(string token, string login, string senhaAtual, string novaSenha);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "PerfisSistema?token={token}")]
        [Description("Retorna lista de todos os perfis associados a um determinado sistema.")]
        List<String> PerfisSistema(string token);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "ObterListaPerfisUsuarioExterno?token={token}&login={login}")]
        [Description("Retorna lista de todos os perfis associados a um determinado usuário.")]
        List<String> ObterListaPerfisUsuarioExterno(string token, string login);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "VerificacaoPeriodicaServico")]
        [Description("Retorna informações específicas para o monitorador de sistemas.")]
        RetornoVerificacaoServico VerificacaoPeriodicaServico();

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "CriarUsuario")]
        [Description("Cria uma conta de usuário com envio de senha temporária.")]
        void CriarUsuarioExterno(string token, Usuario usuario);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "CadastrarUsuario")]
        [Description("Cadastra uma conta de usuário sem envio de senha temporária.")]
        void CriarUsuario(string token, Usuario usuario, string senha);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AssociarUsuarioAoSistemaPerfil")]
        [Description("Associa um usuário a um sistema perfil.")]
        [FaultContract(typeof(UsuarioInexistenteFault))]
        [FaultContract(typeof(TokenInvalidoFault))]
        void AssociarUsuarioAoSistemaPerfil(string token, Usuario usuario);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "BuscarUsuarioExterno")]
        [Description("Retorna uma conta de usuário externo.")]
        Usuario BuscarUsuarioExterno(string token, string login);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AtivarDesativarUsuarioExterno")]
        [Description("Ativa ou desativa a conta de um usuário externo.")]
        void AtivarDesativarUsuarioExterno(string token, string login, bool ativo);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "AtivarDesativarUsuarioInterno")]
        [Description("Ativa ou desativa a conta de um usuário interno.")]
        void AtivarDesativarUsuarioInterno(string token, string login, bool ativo);


        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "ObterPerfisSistema?token={token}")]
        [Description("Retorna lista de todos os perfis associados a um determinado sistema.")]
        List<Perfil> ObterPerfisSistema(string token);
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AutenticacaoServico : IAutenticacaoServico
    {
        
        private Corporativo.API.Cliente _corporativo = new Corporativo.API.Cliente(new Corporativo.Models.Credencial("fulano", "153"));
        private string UserHostName { get { return Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName; } }
        public ILog Log { get { return log4net.LogManager.GetLogger("CONTROLEACESSO"); } }

        public AutenticacaoServico()
        {

            Mapper.CreateMap<Corporativo.Models.PessoaFisica, PessoaFisica>()
                .ForMember(dst => dst.Nascimento, m => m.MapFrom(src => src.Nascimento.Data))
                .ForMember(dst => dst.CPF, m => m.MapFrom(src => src.CPF.Numero))
                .ForMember(dst => dst.Sexo, m => m.MapFrom(src => src.Sexo == Corporativo.Models.Sexo.Feminino ? "F" : "M"));

            Mapper.CreateMap<Entidades.Usuario, Usuario>()
                .ForMember(dst => dst.Tipo, m => m.MapFrom(src => TipoUsuario.Interno))
                .ForMember(dst => dst.Nome, m => m.MapFrom(src => src.PessoaFisica.Nome))
                .ForMember(dst => dst.CPF, m => m.MapFrom(src => src.PessoaFisica.CPF.Numero))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => p.Perfis.Any() ? "A" : "D"));
            Mapper.CreateMap<Entidades.UsuarioExterno, Usuario>()
                .ForMember(dst => dst.Senha, src => src.MapFrom(p => p.Senha.Valor))
                .ForMember(dst => dst.Nome, m => m.MapFrom(src => src.PessoaFisica.Nome))
                .ForMember(dst => dst.CPF, m => m.MapFrom(src => src.PessoaFisica.CPF.Numero))
                .ForMember(dst => dst.Tipo, m => m.MapFrom(src => TipoUsuario.Externo))
                .ForMember(dst => dst.ControleAtivo, m => m.MapFrom(src => src.Perfis.Any() ? "A" : "D"));
            Mapper.CreateMap<Entidades.SistemaPerfil, Perfil>()
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.Perfil.Nome));
            Mapper.CreateMap<Entidades.UsuarioSistemaPerfil, Perfil>()
                .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.CodigoPerfil))
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Nome))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => p.Ativo ? "A" : "D"));
            Mapper.CreateMap<Entidades.UsuarioExternoSistemaPerfil, Perfil>()
                .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.CodigoPerfil))
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Nome))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => p.Ativo ? "A" : "D"))
                ;
        }
        public List<Usuario> TodosUsuarios(string token)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                 if (SistemaServico.Instancia.ValidarToken(token))
               {
                   var lista = UsuarioServico.Instancia.Buscar().ToList();
                   _corporativo.PessoasFisicas.Get(lista, "IdPessoaFisica", "PessoaFisica");
                   return lista.Select(Mapper.Map<Usuario>).ToList();
                  //  return new List<Usuario>{ new Usuario{ Nome = new Random().Next().ToString() } };
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
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);
                    var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals(origem.CodigoSistema)).FirstOrDefault();

                    var buscarExternos = filtro == FiltroObterTodosUsuariosEnum.Todos || filtro == FiltroObterTodosUsuariosEnum.Externos;
                    var buscarInternos = filtro == FiltroObterTodosUsuariosEnum.Todos || filtro == FiltroObterTodosUsuariosEnum.Internos;
                    var usuarios = default(List<Usuario>);
                    var usuariosEntidades = default(IEnumerable<ControleAcesso.Dominio.Entidades.Usuario>);
                    var usuariosExternosEntidades = default(IEnumerable<ControleAcesso.Dominio.Entidades.UsuarioExterno>);

                    System.Linq.Expressions.Expression<Func<Entidades.Usuario, bool>> criterio = null;
                    System.Linq.Expressions.Expression<Func<Entidades.UsuarioExterno, bool>> criterioExterno = null;
                    if (filtrarPorSistema)
                    {
                        criterio = i => i.Perfis.Any(p => p.CodigoSistema == sistema.Codigo);
                        criterioExterno = i => i.Perfis.Any(p => p.CodigoSistema == sistema.Codigo);
                    }

                    if (buscarInternos)
                    {
                        usuariosEntidades = UsuarioServico.Instancia.Buscar(criterio);
                        
                        _corporativo.PessoasFisicas.Get(usuariosEntidades, "IdPessoaFisica", "PessoaFisica");
                        
                        usuarios = usuariosEntidades.Select(Mapper.Map<Usuario>).ToList();

                        if (buscarExternos)
                            usuariosExternosEntidades = UsuarioExternoServico.Instancia.Buscar(criterioExterno);

                        _corporativo.PessoasFisicas.Get(usuariosExternosEntidades, "IdPessoaFisica", "PessoaFisica");

                        if (usuariosExternosEntidades != null && usuariosExternosEntidades.Count() > 0)
                            usuarios = usuarios.Union(usuariosExternosEntidades.Select(Mapper.Map<Usuario>)).ToList();

                    }
                    else
                    {
                        usuarios = UsuarioExternoServico.Instancia.Buscar(criterioExterno).Select(Mapper.Map<Usuario>).ToList();
                    }

                    usuarios.ForEach(u => u.Perfis.RemoveAll(p => p.ControleAtivo == "D"));

                    return usuarios;
                }
                else
                {
                    var message = string.Format("Tentativa de acessar o método 'ObterTodosUsuarios' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName);
                    Log.Warn(message);
                    var fault = new TokenInvalidoFault() { Token = token, Message = message };
                    throw new FaultException<TokenInvalidoFault>(fault);
                }

            }
            catch (TokenInvalidoException ex)
            {

                Log.Error(ex.Message, ex);
                var fault = new TokenInvalidoFault() { Token = token, Message = ex.Message };
                throw new FaultException<TokenInvalidoFault>(fault);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        private IEnumerable<Perfil> CreatePerfis(Entidades.Usuario usuario, IEnumerable<Entidades.Perfil> todosPerfis)
        {
            var list = new List<Perfil>();
            foreach (var item in usuario.Perfis)
            {
                var perfil = todosPerfis.FirstOrDefault(i => i.Codigo == item.CodigoPerfil);
                if (perfil != null)
                    list.Add(new Perfil() { CodigoPerfil = item.CodigoPerfil, NomePerfil = perfil.Nome, CodigoSistema = item.CodigoSistema });
            }
            return list;
        }

        public void CriarUsuarioExterno(string token, Usuario usuario)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);

                    ValidaEmail(usuario.Email);

                    var usuarioEx = new Entidades.UsuarioExterno
                    {
                        Email = usuario.Email,
                        IdPessoaFisica = usuario.PessoaFisica.Id,
                        Login = usuario.Email,
                        Nome = usuario.Nome,
                        Ativo = true,

                    };
                    var perfis = usuario.Perfis.Select(p => new ControleAcesso.Dominio.Entidades.SistemaPerfil
                    {
                        CodigoSistema = origem.CodigoSistema
                    ,
                        CodigoPerfil = p.CodigoPerfil
                    });

                    foreach (var item in perfis)
                    {
                        if (!usuarioEx.Perfis.Any(i => i.CodigoPerfil != null && i.CodigoPerfil.Trim() == item.CodigoPerfil.Trim()))
                            usuarioEx.AdicionarPerfil(item);
                    }

                    var usuarioServico = new UsuarioExternoServico();
                    usuarioServico.Salvar(usuarioEx);

                    string senhaTemporaria = usuarioServico.SolicitarSenhaTemporaria(usuarioEx.Login, DateTime.Now.AddDays(AppSettings.PrazoExpiracaoSenhaTemporaria)); // -1));

                    EnviarEmail(
                        usuario.Email,
                        "[" + origem.CodigoSistema + "] Cadastro efetuado com sucesso",
                        "~/EmailTemplates/" + origem.CodigoSistema + "Cadastro.cshtml",
                        new EmailData
                        {
                            Login = usuario.Email,
                            Senha = senhaTemporaria,
                            Prazo = ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"]
                        });
                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'CriarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                Log.Error(ex.Message, ex);
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException)
                {
                    var sqlEx = (System.Data.SqlClient.SqlException)ex.InnerException;
                    if (sqlEx.Message.Contains("Violation of PRIMARY KEY") && sqlEx.Message.Contains("CONTROLEACESSO.TB_LOGIN_EXTERNO"))
                    {
                        throw new ControleAcesso.Dominio.Aplicacao.Exceptions.UsuarioCadastradoException();
                    }
                }

                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void CriarUsuario(string token, Usuario usuario, string senha)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);

                    ValidaEmail(usuario.Email);

                    var usuarioEx = new Entidades.UsuarioExterno
                    {
                        Email = usuario.Email,
                        IdPessoaFisica = usuario.PessoaFisica.Id,
                        Login = usuario.Email,
                        Nome = usuario.Nome,
                        Ativo = true,
                    };

                    var perfis = usuario.Perfis.Select(p => new ControleAcesso.Dominio.Entidades.SistemaPerfil
                    {
                        CodigoSistema = origem.CodigoSistema,
                        CodigoPerfil = p.CodigoPerfil
                    });

                    foreach (var item in perfis)
                    {
                        if (!usuarioEx.Perfis.Any(i => i.CodigoPerfil != null && i.CodigoPerfil.Trim() == item.CodigoPerfil.Trim()))
                            usuarioEx.AdicionarPerfil(item);
                    }

                    var usuarioServico = new UsuarioExternoServico();

                    using (var trans = new TransactionScope())
                    {
                        usuarioEx.CriarSenha(senha);
                        usuarioServico.Salvar(usuarioEx);

                        trans.Complete();
                    }

                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'CadastrarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                Log.Error(ex.Message, ex);
                if (ex.InnerException != null && ex.InnerException is System.Data.SqlClient.SqlException)
                {
                    var sqlEx = (System.Data.SqlClient.SqlException)ex.InnerException;
                    if (sqlEx.Message.Contains("Violation of PRIMARY KEY") && sqlEx.Message.Contains("CONTROLEACESSO.TB_LOGIN_EXTERNO"))
                    {
                        throw new ControleAcesso.Dominio.Aplicacao.Exceptions.UsuarioCadastradoException();
                    }
                }

                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public void AtualizarPerfis(string token, string login, List<string> perfis)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var codigoSistema = SistemaServico.Instancia.DecomporToken(token).CodigoSistema;
                    var usuario = UsuarioServico.Instancia.Buscar(u => u.Login.ToUpper() == login.Trim().ToUpper()).FirstOrDefault();

                    if (usuario != null)
                    {
                        usuario.Contextualizar(codigoSistema);
                        var remover = usuario.Perfis.Where(p => perfis.All(pn => p.CodigoPerfil.Trim().ToUpper() != pn.Trim().ToUpper())).ToList();
                        var adicionar = perfis.Where(pn => usuario.Perfis.All(p => pn.Trim().ToUpper() != p.CodigoPerfil.Trim().ToUpper())).ToList();

                        remover.ForEach(p => usuario.RemoverPerfil(p.SistemaPerfil));
                        foreach (var p in adicionar)
                        {
                            var perfil = new Entidades.SistemaPerfil()
                            {
                                Ativo = true,
                                Alteracao = DateTime.Now,
                                CodigoSistema = codigoSistema,
                                CodigoPerfil = p,
                                Uso = "I",
                                Origem = "I",
                            };
                            usuario.AdicionarPerfil(perfil);
                        }

                        UsuarioServico.Instancia.Salvar(usuario);
                    }
                    else
                    {
                        var usuarioExterno = UsuarioExternoServico.Instancia.Buscar(u => u.Login.ToUpper() == login.Trim().ToUpper()).FirstOrDefault();
                        if (usuarioExterno == null) throw new NotFoundException();

                        usuarioExterno.Contextualizar(codigoSistema);
                        var remover = usuarioExterno.Perfis.Where(p => perfis.All(pn => p.CodigoPerfil.Trim().ToUpper() != pn.Trim().ToUpper())).ToList();
                        var adicionar = perfis.Where(pn => usuarioExterno.Perfis.All(p => pn.Trim().ToUpper() != p.CodigoPerfil.Trim().ToUpper())).ToList();

                        remover.ForEach(p => usuarioExterno.RemoverPerfil(p.SistemaPerfil));
                        foreach (var p in adicionar)
                        {
                            var perfil = new Entidades.SistemaPerfil()
                            {
                                Ativo = true,
                                Alteracao = DateTime.Now,
                                CodigoSistema = codigoSistema,
                                CodigoPerfil = p,
                                Uso = "I",
                                Origem = "I",
                            };
                            usuarioExterno.AdicionarPerfil(perfil);
                        }

                        UsuarioExternoServico.Instancia.SalvarComTransacao(usuarioExterno);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        public void CriarSenha(string token, string login, string senha)
        {
            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    UsuarioExternoServico.Instancia.CriarSenha(login, senha);
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
        #region Usuário Interno
        public Usuario AutenticarUsuario(string token, Login login)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    try
                    {
                        var origem = SistemaServico.Instancia.DecomporToken(token);
                        var usuario = UsuarioServico.Instancia.AutenticarUsuario(origem.CodigoSistema, login.UserName, login.Senha);
                        usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
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
        #endregion
        #region Usuário Anonimo
        public Usuario AutenticarUsuarioAnonimo(string token, string credencial)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                try
                {

                    if (credencial == ConfigurationManager.AppSettings["CredencialAcessoAnonimo"])
                    {

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
                var objToken = SistemaServico.Instancia.DecomporToken(token);
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
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var usuario = UsuarioExternoServico.Instancia.Buscar(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();
                    usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
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
                var objToken = SistemaServico.Instancia.DecomporToken(token);
                if (token != null)
                {
                    var sistema = SistemaServico.Instancia.Buscar(s => s.Codigo.Equals(objToken.CodigoSistema)).FirstOrDefault();
                    var perfis = sistema.PerfisAcesso;
                    return perfis.Where(p => p.Ativo).Select(p => new Perfil() { CodigoPerfil = p.Codigo.Trim(), CodigoSistema = sistema.Codigo.Trim(), NomePerfil = p.Nome.Trim() }).ToList();
                }

                return new List<Perfil>();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion

        #region Autenticar
        public Usuario Autenticar(string token, Login login)
        {
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);
                    if (EhUsuarioInterno(login.UserName))
                    {
                        return AutenticarUsuarioInterno(login, origem);
                        
                    }

                    return  AutenticarUsuarioExterno(login, origem);
                }

                    Log.Warn(
                        string.Format(
                            "Tentativa de acessar o método 'AutenticarUsuario' com token inválido ({0}), a partir da máquina {1}. ",
                            token, UserHostName));
                    throw new Exception("Token inválido.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }
        #endregion

        #region Usuário externo
        public Usuario AutenticarUsuarioExterno(string token, Login login)
        {
            ControleAcesso.Dominio.ObjetosDeValor.ServidorOrigem origem = null;
            log4net.GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    origem = SistemaServico.Instancia.DecomporToken(token);
                    var usuario = UsuarioExternoServico.Instancia.Autenticar(origem.CodigoSistema, login.UserName, login.Senha);
                    usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
                    return Mapper.Map<Entidades.UsuarioExterno, Usuario>(usuario);
                }

                Log.Warn(string.Format("Tentativa de acessar o método 'AutenticarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                throw new Exception("Token inválido.");
            }
            catch (SenhaTemporariaExpiradaException ex)
            {
                EnviarEmail(
                        login.UserName,
                        "[" + origem.CodigoSistema + "] Nova senha",
                        "~/EmailTemplates/" + origem.CodigoSistema + "SenhaTemporaria.cshtml",
                        new EmailData
                        {
                            Login = login.UserName,
                            Senha = ex.NovaSenha,
                            Prazo = ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"]
                        });
                Log.Error(ex.Message, ex);
                throw ex;
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
                if (SistemaServico.Instancia.ValidarToken(token))
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
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var sistema = SistemaServico.Instancia.BuscarPorToken(token).Codigo;
                    var senha = UsuarioExternoServico.Instancia.SolicitarSenhaTemporaria(login);
                    EnviarEmail(
                        login,
                        "[" + sistema + "] Nova senha",
                        "~/EmailTemplates/" + sistema + "SenhaTemporaria.cshtml",
                        new EmailData
                        {
                            Login = login,
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
                    if (SistemaServico.Instancia.ValidarToken(token))
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

        private void EnviarEmail(string para, string assunto, string urlTemplate, EmailData modelo)
        {
            try
            {
                var rm = new RazorMachine();
                var template = rm.ExecuteUrl(urlTemplate, modelo);
                var corpo = template.Result;

                MailMessage mail = new MailMessage("controleacesso-naoresponda@inmetro.gov.br", para);
                mail.Subject = assunto;
                mail.IsBodyHtml = true;
                mail.Body = corpo;

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("controleacesso-naoresponda", "$s9KGrM7Kt");
                client.Host = "webmail.inmetro.gov.br";
                client.Port = 25;
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
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);
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
                    var fault = new TokenInvalidoFault { Token = token, Message = message };
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
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);
                    var usuarioex = UsuarioExternoServico.Instancia.Buscar(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();
                    if (usuarioex != null)
                    {
                        string cod = origem.CodigoSistema;
                        origem = null;
                        usuarioex.AtivarDesativarTodosPerfis(cod, ativo);
                        UsuarioExternoServico.Instancia.Salvar(usuarioex);


                        //var usuarioServico = new UsuarioExternoServico();

                        ////using (var trans = new TransactionScope())
                        ////{

                        //    usuarioServico.Salvar(usuarioex);

                        ////    trans.Complete();
                        ////}

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

                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    var origem = SistemaServico.Instancia.DecomporToken(token);
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

        public Usuario AutenticarUsuarioInterno(Login login, ServidorOrigem origem)
        {
            try
            {
                var usuario = UsuarioServico.Instancia.AutenticarUsuario(origem.CodigoSistema, login.UserName, login.Senha);
                usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
                return Mapper.Map<Entidades.Usuario, Usuario>(usuario);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public Usuario AutenticarUsuarioExterno(Login login, ServidorOrigem origem)
        {
            try
            {
                var usuario = UsuarioExternoServico.Instancia.Autenticar(origem.CodigoSistema, login.UserName, login.Senha);
                usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
              
              
                return Mapper.Map<Entidades.UsuarioExterno, Usuario>(usuario);
            }
            catch (SenhaTemporariaExpiradaException ex)
            {
                EnviarEmail(
                      login.UserName,
                      "[" + origem.CodigoSistema + "] Nova senha",
                      "~/EmailTemplates/" + origem.CodigoSistema + "SenhaTemporaria.cshtml",
                      new EmailData
                      {
                          Login = login.UserName,
                          Senha = ex.NovaSenha,
                          Prazo = ConfigurationManager.AppSettings["PrazoExpiracaoSenhaTemporaria"]
                      });
                Log.Error(ex.Message, ex);
                throw ex;
            }
        }

        private void ValidaEmail(string emailUsuario)
        {
            bool email = ValidarEmail.Validar(emailUsuario);
            if (!email)
            {
                throw new EmailInvalidoException();
            }

        }

        private bool EhUsuarioInterno(string login)
        {
            ActiveDirectoryHelper adHelper = new ActiveDirectoryHelper("");
            var user = adHelper.GetUserByLoginName(login);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public Usuario ObterUsuario(string token, string login)
        {
            log4net.GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.Instancia.ValidarToken(token))
                {
                    Usuario usuario = null;
                    var tipo = login.Contains("@") ? TipoUsuario.Externo : TipoUsuario.Interno;

                    if (tipo == TipoUsuario.Interno)
                    {
                        var usr = UsuarioServico.Instancia.Buscar(u => u.Login.Trim().ToUpper() == login.Trim().ToUpper()).FirstOrDefault();
                        if (usr != null)
                        {
                            var pessoa = _corporativo.PessoasFisicas.Get(usr.IdPessoaFisica);
                            if (pessoa != null)
                            {
                                usr.PessoaFisica = pessoa;
                                usuario = Mapper.Map<Usuario>(usr);
                            }
                        }
                    }
                    else
                    {
                        var usr = UsuarioExternoServico.Instancia.Buscar(u => u.Login.Trim().ToUpper() == login.Trim().ToUpper()).FirstOrDefault();
                        if (usr != null)
                        {
                            var pessoa = _corporativo.PessoasFisicas.Get(usr.IdPessoaFisica);
                            if (pessoa != null)
                            {
                                usr.PessoaFisica = pessoa;
                                usuario = Mapper.Map<Usuario>(usr);
                            }
                        }
                    }

                    usuario.Perfis.RemoveAll(p => p.ControleAtivo == "D");

                    return usuario;
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

        public object CriarUsuarioExterno(string token, Login login)
        {
            throw new NotImplementedException();
        }

        public object CriarUsuarioExterno(string token, Entidades.UsuarioExterno usuario)
        {
            throw new NotImplementedException();
        }

        public object ObterTodosUsuarios(string token, int filtro, bool filtrarPorSistema)
        {
            throw new NotImplementedException();
        }
    }
}