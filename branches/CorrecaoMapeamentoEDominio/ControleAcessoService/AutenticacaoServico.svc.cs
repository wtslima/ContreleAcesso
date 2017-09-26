using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Configuration;
using System.ServiceModel.Web;
using System.Text;
using System.Transactions;
using System.Web;
using AutoMapper;
using Castle.Core.Internal;
using Castle.Windsor;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Exceptions;
using ControleAcesso.Dominio.Helpers;
using ControleAcesso.Dominio.ObjetosDeValor;
using ControleAcesso.Infra.IoC;
using ControleAcessoService.Configuration;
using ControleAcessoService.DataContracts;
using ControleAcessoService.Elemento;
using Corporativo.API;
using Corporativo.Exceptions;
using Corporativo.Models;
using Corporativo.Utils.ActiveDirectory;
using Inmetro.Utils.Linq.ExtensionMethods;
using log4net;
using NHibernate.Exceptions;
using NHibernate.Hql.Ast.ANTLR;
using Xipton.Razor;
using Xipton.Razor.Core;
using Xipton.Razor.Extension;
using Perfil = ControleAcessoService.DataContracts.Perfil;
using PessoaFisica = Corporativo.Models.PessoaFisica;
using Usuario = ControleAcessoService.DataContracts.Usuario;


namespace ControleAcessoService
{
    [ServiceContract(Namespace = "http://inmetro.gov.br/ControleAcesso")]
    public interface IAutenticacaoServico
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
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
                   UriTemplate = "AssociarPerfilSistema")]
        [Description("Associar sistema(s) ao perfil.")]
        void AssociarPerfilSistema(string token, List<SistemaPerfil> lstSisPerfil);

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
        [WebInvoke(Method = "POST",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest,
                   UriTemplate = "CadastrarSistema")]
        [Description("Cadastra um sistema.")]
        void CriarSistema(DataContracts.Sistema objeto);


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
        IUsuarioServicoApp UsuarioServico { get; set; }
        IUsuarioExternoServicoApp UsuarioExternoServico { get; set; }
        ISistemaServicoApp SistemaServico { get; set; }
        ISistemaPerfilServicoApp SistemaPerfilServico { get; set; }
        IPerfilServicoApp PerfilServico { get; set; }

        private Cliente _corporativo = new Cliente(new Credencial("fulano", "153"));
        private string UserHostName { get { return Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName; } }
        public ILog Log { get { return LogManager.GetLogger("CONTROLEACESSO"); } }

        public AutenticacaoServico()
        {

            var container = new WindsorContainer();
            new ConfigurarDependencias().Install(container);
            UsuarioServico = container.Resolve<IUsuarioServicoApp>();
            UsuarioExternoServico = container.Resolve<IUsuarioExternoServicoApp>();
            SistemaServico = container.Resolve<ISistemaServicoApp>();
            PerfilServico = container.Resolve<IPerfilServicoApp>();

            SistemaPerfilServico = container.Resolve<ISistemaPerfilServicoApp>();

            Mapper.CreateMap<PessoaFisica, DataContracts.PessoaFisica>()
                .ForMember(dst => dst.Nascimento, m => m.MapFrom(src => src.Nascimento.Data))
                .ForMember(dst => dst.CPF, m => m.MapFrom(src => src.CPF.Numero))
                .ForMember(dst => dst.Sexo, m => m.MapFrom(src => src.Sexo == Sexo.Feminino ? "F" : "M"));

            Mapper.CreateMap<ControleAcesso.Dominio.Entidades.Usuario, Usuario>()
                .ForMember(dst => dst.Id, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.Tipo, m => m.MapFrom(src => TipoUsuario.Interno))
                .ForMember(dst => dst.Nome, m => m.MapFrom(src => src.PessoaFisica.Nome))
                .ForMember(dst => dst.CPF, m => m.MapFrom(src => src.PessoaFisica.CPF.Numero))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => !p.Perfis.Any()));

            Mapper.CreateMap<UsuarioExterno, Usuario>()
                .ForMember(dst => dst.Id, m => m.MapFrom(src => src.Id))
                .ForMember(dst => dst.Senha, src => src.MapFrom(p => p.Senha.Valor))
                .ForMember(dst => dst.Nome, m => m.MapFrom(src => src.PessoaFisica.Nome))
                .ForMember(dst => dst.CPF, m => m.MapFrom(src => src.PessoaFisica.CPF.Numero))
                .ForMember(dst => dst.Tipo, m => m.MapFrom(src => TipoUsuario.Externo))
                .ForMember(dst => dst.ControleAtivo, m => m.MapFrom(src => !src.Perfis.Any()));

            Mapper.CreateMap<SistemaPerfil, Perfil>()
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.Perfil.Nome));

            //Mapper.CreateMap<SistemaPerfil, SistemaPerfil>()
            //    .ForMember(dst => dst.Id, src => src.MapFrom(p => p.Id))
            //    .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.CodigoPerfil))
            //    .ForMember(dst => dst.CodigoSistema, src => src.MapFrom(p => p.CodigoSistema))
            //    .ForMember(dst => dst.Excluido, src => src.MapFrom(p => p.Excluido))
            //    .ForMember(dst => dst.Origem, src => src.MapFrom(p => p.Origem))
            //    ;


            Mapper.CreateMap<UsuarioSistemaPerfil, Perfil>()
                .ForMember(dst => dst.Id, src => src.MapFrom(p => p.IdPerfil))
                .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Codigo))
                .ForMember(dst => dst.CodigoSistema, src => src.MapFrom(p => p.SistemaPerfil.Sistema.Codigo))
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Nome))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => p.CodigoAtivo.Trim().ToUpper() == "A" ? "A" : "D"));

            Mapper.CreateMap<UsuarioExternoSistemaPerfil, Perfil>()
                .ForMember(dst => dst.Id, src => src.MapFrom(p => p.IdPerfil))
                .ForMember(dst => dst.CodigoSistema, src => src.MapFrom(p => p.SistemaPerfil.Sistema.Codigo))
                .ForMember(dst => dst.CodigoPerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Codigo))
                .ForMember(dst => dst.NomePerfil, src => src.MapFrom(p => p.SistemaPerfil.Perfil.Nome))
                .ForMember(dst => dst.ControleAtivo, src => src.MapFrom(p => !p.Excluido));

            Mapper.CreateMap<ControleAcesso.Dominio.Entidades.Sistema, DataContracts.Sistema>()
                .ForMember(dst => dst.Id, src => src.MapFrom(p => p.Id))
                .ForMember(dst => dst.Codigo, src => src.MapFrom(p => p.Codigo))
                .ForMember(dst => dst.Nome, src => src.MapFrom(p => p.Nome));


        }

        public List<Usuario> TodosUsuarios(string token)
        {
            GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var lista = UsuarioServico.Buscar().ToList();
                    _corporativo.PessoasFisicas.Get(lista, "IdPessoaFisica", "PessoaFisica");
                    return Enumerable.ToList(lista.Select(Mapper.Map<Usuario>));
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
            GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);
                    var sistema = SistemaServico.Buscar(s => s.Id.Equals(origem.CodigoSistema)).FirstOrDefault();

                    var buscarExternos = filtro == FiltroObterTodosUsuariosEnum.Todos || filtro == FiltroObterTodosUsuariosEnum.Externos;
                    var buscarInternos = filtro == FiltroObterTodosUsuariosEnum.Todos || filtro == FiltroObterTodosUsuariosEnum.Internos;
                    var usuarios = default(List<Usuario>);
                    var usuariosEntidades = default(IEnumerable<ControleAcesso.Dominio.Entidades.Usuario>);
                    var usuariosExternosEntidades = default(IEnumerable<UsuarioExterno>);

                    Expression<Func<ControleAcesso.Dominio.Entidades.Usuario, bool>> criterio = null;
                    Expression<Func<UsuarioExterno, bool>> criterioExterno = null;
                    if (filtrarPorSistema && sistema != null)
                    {
                        criterio = i => i.Perfis.Any(p => p.IdSistema == sistema.Id);
                        criterioExterno = i => i.Perfis.Any(p => p.IdSistema == sistema.Id);
                    }

                    if (buscarInternos)
                    {
                        usuariosEntidades = UsuarioServico.Buscar(criterio);
                        _corporativo.PessoasFisicas.Get(usuariosEntidades, "IdPessoaFisica", "PessoaFisica");
                        usuarios = Enumerable.ToList(usuariosEntidades.Select(Mapper.Map<Usuario>));
                        if (buscarExternos)
                            usuariosExternosEntidades = UsuarioExternoServico.Buscar(criterioExterno);
                        _corporativo.PessoasFisicas.Get(usuariosExternosEntidades, "IdPessoaFisica", "PessoaFisica");

                        if (usuariosExternosEntidades != null && usuariosExternosEntidades.Count() > 0)
                            usuarios = usuarios.Union(usuariosExternosEntidades.Select(Mapper.Map<Usuario>)).ToList();

                    }
                    else
                    {
                        usuarios = Enumerable.ToList(UsuarioExternoServico.Buscar(criterioExterno).Select(Mapper.Map<Usuario>));
                    }

                    return usuarios;
                }
                var message = string.Format("Tentativa de acessar o método 'ObterTodosUsuarios' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName);
                Log.Warn(message);
                var fault = new TokenInvalidoFault() { Token = token, Message = message };
                throw new FaultException<TokenInvalidoFault>(fault);
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
        //TODO: Reutilizar método na camada de aplicação ou domínio para criação de perfis;
        //private IEnumerable<Perfil> CreatePerfis(Entidades.Usuario usuario, IEnumerable<Entidades.Perfil> todosPerfis)
        //{
        //    var list = new List<Perfil>();
        //    foreach (var item in usuario.Perfis)
        //    {
        //        var perfil = todosPerfis.FirstOrDefault(i => i.Codigo == item.CodigoPerfil);
        //        if (perfil != null)
        //            list.Add(new Perfil() { CodigoPerfil = item.CodigoPerfil, NomePerfil = perfil.Nome, CodigoSistema = item.CodigoSistema });
        //    }
        //    return list;
        //}

        public void CriarUsuarioExterno(string token, Usuario usuario)
        {
            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);

                    ValidaEmail(usuario.Email);

                    var usuarioEx = new UsuarioExterno
                    {
                        Email = usuario.Email,
                        IdPessoaFisica = usuario.PessoaFisica.Id,
                        Login = usuario.Email,
                        Nome = usuario.Nome,
                        Excluido = false,
                    };
                    var perfis = usuario.Perfis.Select(p => new SistemaPerfil
                    {
                        Perfil = new ControleAcesso.Dominio.Entidades.Perfil
                        {
                            Codigo = p.CodigoPerfil
                        }
                    });


                    foreach (var item in perfis)
                    {
                        var resultado = SistemaServico.BuscarTodosSistemasPorCodigoPerfil(item.Perfil.Codigo);

                        foreach (var retorno in resultado.SelectMany(d => d.PerfisAcesso))
                        {
                            var sistemaPerfil = new SistemaPerfil() { CodigoPerfil = retorno.Id, CodigoSistema = origem.CodigoSistema };
                            usuarioEx.AdicionarPerfil(sistemaPerfil);
                        }
                    }

                    UsuarioExternoServico.Cadastrar(usuarioEx);

                    string senhaTemporaria = UsuarioExternoServico.SolicitarSenhaTemporaria(usuarioEx.Login, DateTime.Now.AddDays(AppSettings.PrazoExpiracaoSenhaTemporaria));

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
            catch (GenericADOException ex)
            {
                Log.Error(ex.Message, ex);
                if (ex.InnerException != null && ex.InnerException is SqlException)
                {
                    var sqlEx = (SqlException)ex.InnerException;
                    if (sqlEx.Message.Contains("Violation of PRIMARY KEY") && sqlEx.Message.Contains("CONTROLEACESSO.TB_LOGIN_EXTERNO"))
                    {
                        throw new UsuarioCadastradoException();
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


        public void AssociarPerfilSistema(string token, List<SistemaPerfil> lstSisPerfil)
        {
            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    using (var trans = new TransactionScope())
                    {
                        SistemaPerfilServico.AssociarPerfilSistema(lstSisPerfil);

                        trans.Complete();
                    }

                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'AssociarPerfilSistema' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (GenericADOException ex)
            {
                Log.Error(ex.Message, ex);
                if (ex.InnerException != null && ex.InnerException is SqlException)
                {
                    var sqlEx = (SqlException)ex.InnerException;
                    if (sqlEx.Message.Contains("Violation of PRIMARY KEY") && sqlEx.Message.Contains("CONTROLEACESSO.TB_SISTEMA_PERFIL"))
                    {
                        throw new UsuarioCadastradoException();
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
                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);

                    ValidaEmail(usuario.Email);

                    var usuarioEx = new UsuarioExterno
                    {
                        Email = usuario.Email,
                        IdPessoaFisica = usuario.PessoaFisica.Id,
                        Login = usuario.Email,
                        Nome = usuario.Nome,
                        Excluido = false,
                    };

                    var perfis = usuario.Perfis.Select(p => new SistemaPerfil
                    {
                        Perfil = new ControleAcesso.Dominio.Entidades.Perfil
                        {
                            Codigo = p.CodigoPerfil
                        }
                    });

                   
                    foreach (var item in perfis)
                    {
                        var resultado = SistemaServico.BuscarTodosSistemasPorCodigoPerfil(item.Perfil.Codigo);

                        foreach (var retorno in resultado.SelectMany(d => d.PerfisAcesso))
                        {
                            var sistemaPerfil = new SistemaPerfil() {CodigoPerfil = retorno.Id  , CodigoSistema = origem.CodigoSistema };
                            usuarioEx.AdicionarPerfil(sistemaPerfil);
                        }
                           
                    }
                    using (var trans = new TransactionScope())
                    {
                        usuarioEx.AdicionarSenha(senha, DateTime.Now, true);
                        UsuarioExternoServico.Salvar(usuarioEx);

                        trans.Complete();
                    }

                }
                else
                {
                    Log.Warn(string.Format("Tentativa de acessar o método 'CadastrarUsuario' com token inválido ({0}), a partir da máquina {1}. ", token, UserHostName));
                    throw new Exception("Token inválido.");
                }
            }
            catch (GenericADOException ex)
            {
                Log.Error(ex.Message, ex);
                if (ex.InnerException != null && ex.InnerException is SqlException)
                {
                    var sqlEx = (SqlException)ex.InnerException;
                    if (sqlEx.Message.Contains("Violation of PRIMARY KEY") && sqlEx.Message.Contains("CONTROLEACESSO.TB_LOGIN_EXTERNO"))
                    {
                        throw new UsuarioCadastradoException();
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

        public void CriarSistema(DataContracts.Sistema objeto)
        {
            var novoSistema = new ControleAcesso.Dominio.Entidades.Sistema
            {
                Id = objeto.Id,
                Codigo = objeto.Codigo,
                Nome = objeto.Nome,
                Excluido = false,
                Origem = "I",
                Alteracao = DateTime.Now
            };
            SistemaServico.Cadastrar(novoSistema);
        }
        public void CriarPerfil(Perfil objeto)
        {
            var novoPerfil = new ControleAcesso.Dominio.Entidades.Perfil()
            {
                Excluido = false,
                Alteracao = DateTime.Now,
                Codigo = objeto.CodigoPerfil,
                Nome = objeto.NomePerfil,
                Descricao = objeto.NomePerfil.ToUpper(),
                Origem = "I"
            };

            PerfilServico.Cadastrar(novoPerfil);

        }
        public void AtualizarPerfis(string token, string login, List<string> perfis)
        {

            GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);
            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var codigoSistema = SistemaServico.DecomporToken(token).CodigoSistema;
                    var usuario = UsuarioServico.Buscar(u => u.Login.ToUpper() == login.Trim().ToUpper()).FirstOrDefault();

                    if (usuario != null)
                    {
                        usuario.Contextualizar(codigoSistema);
                        var remover = usuario.Perfis.Where(p => perfis.All(pn => p.SistemaPerfil.Perfil.Codigo != pn.ToString())).ToList();
                        var adicionar = perfis.Where(pn => usuario.Perfis.All(p => pn != p.IdPerfil.ToString())).ToList();

                        remover.ForEach(p => usuario.RemoverPerfil(p.SistemaPerfil));
                        foreach (var p in adicionar)
                        {
                            var perfil = new SistemaPerfil()
                            {
                                Excluido = false,
                                Alteracao = DateTime.Now,
                                CodigoSistema = codigoSistema,
                                CodigoPerfil = Convert.ToInt32(p),
                                Origem = "I",
                            };
                            usuario.AdicionarPerfil(perfil);
                        }

                        UsuarioServico.Salvar(usuario);
                       
                    }
                    else
                    {
                        var usuarioExterno = UsuarioExternoServico.Buscar(u => u.Login.ToUpper() == login.Trim().ToUpper()).FirstOrDefault();
                        if (usuarioExterno == null) throw new NotFoundException();

                        usuarioExterno.Contextualizar(codigoSistema);
                        var remover = usuarioExterno.Perfis.Where(p => perfis.All(pn => p.SistemaPerfil.Perfil.Codigo != pn)).ToList();
                        var adicionar = perfis.Where(pn => usuarioExterno.Perfis.All(p => pn != p.SistemaPerfil.Perfil.Codigo)).ToList();

                        remover.ForEach(p => usuarioExterno.RemoverPerfil(p.SistemaPerfil));
                        foreach (var p in adicionar)
                        {
                            var perfil = new SistemaPerfil()
                            {
                                Excluido = false,
                                Alteracao = DateTime.Now,
                                CodigoSistema = codigoSistema,
                                CodigoPerfil = Convert.ToInt32(p),
                                Origem = "I",
                            };
                            usuarioExterno.AdicionarPerfil(perfil);
                        }

                        UsuarioExternoServico.Salvar(usuarioExterno);
                        
                    }
                }
               
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message, ex);
               
                throw;
            }
        }
        
        #region Usuário Interno
        public Usuario AutenticarUsuario(string token, Login login)
        {
            GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    try
                    {
                        var origem = SistemaServico.DecomporToken(token);
                        var usuario = UsuarioServico.AutenticarUsuario(origem.CodigoSistema, login.UserName, login.Senha);
                        usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
                        return Mapper.Map<ControleAcesso.Dominio.Entidades.Usuario, Usuario>(usuario);
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
            GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                try
                {
                    if (credencial == ConfigurationManager.AppSettings["CredencialAcessoAnonimo"])
                    {

                        var usuario = new ControleAcesso.Dominio.Entidades.Usuario { Login = "Consumidor@Servir" };

                        return Mapper.Map<ControleAcesso.Dominio.Entidades.Usuario, Usuario>(usuario);
                    }
                    throw new NaoAutorizadoException();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    throw;
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
            GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                var objToken = SistemaServico.DecomporToken(token);
                if (token != null)
                {
                    var resultado = SistemaServico.Buscar(s => s.Id.Equals(objToken.CodigoSistema)).FirstOrDefault();
                    if (resultado != null)
                    {
                        var perfis = resultado.PerfisAcesso;
                        return perfis.Where(p => p.Excluido).Select(p => p.Codigo.Trim()).ToList();
                    }
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
            GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var usuario = UsuarioExternoServico.Buscar(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();
                    usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
                    return Mapper.Map<UsuarioExterno, Usuario>(usuario);
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
            GlobalContext.Properties["Request"] = string.Format("token={0}", token);

            try
            {
                var objToken = SistemaServico.DecomporToken(token);
                if (token != null)
                {
                    var sistema = SistemaServico.Buscar(s => s.Id.Equals(objToken.CodigoSistema)).FirstOrDefault();
                    var perfis = sistema.PerfisAcesso;
                    return perfis.Where(p => p.Excluido == false).Select(p => new Perfil() { CodigoPerfil = p.Codigo.Trim(), CodigoSistema = sistema.Codigo.Trim(), NomePerfil = p.Nome.Trim() }).ToList();
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
            GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);
                    if (EhUsuarioInterno(login.UserName))
                    {
                        return AutenticarUsuarioInterno(login, origem);

                    }

                    return AutenticarUsuarioExterno(login, origem);
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
            ServidorOrigem origem = null;
            GlobalContext.Properties["Request"] = string.Format("token={0}, {1}", token, login);

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    origem = SistemaServico.DecomporToken(token);
                    var usuario = UsuarioExternoServico.Autenticar(origem.CodigoSistema, login.UserName, login.Senha);
                    usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
                    return Mapper.Map<UsuarioExterno, Usuario>(usuario);
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
                if (SistemaServico.ValidarToken(token))
                {
                    UsuarioExternoServico.AlterarSenha(login, senhaAtual, novaSenha);
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
                if (SistemaServico.ValidarToken(token))
                {
                    var sistema = SistemaServico.BuscarPorToken(token).Codigo;
                    var senha = UsuarioExternoServico.SolicitarSenhaTemporaria(login);
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
                    if (SistemaServico.ValidarToken(token))
                    {
                        var usuario = UsuarioExternoServico.Buscar(u => u.Login.ToLower().Equals(login.ToLower().Trim())).FirstOrDefault();

                        return usuario.Perfis.Select(p => p.SistemaPerfil.Perfil.Codigo.Trim()).ToList();
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
                client.Credentials = new NetworkCredential("controleacesso-naoresponda", "$s9KGrM7Km");
                client.Host = "wtslima@gmail.com";
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
                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);
                    var usuarioDatabase = UsuarioServico.Buscar(i => i.Login == usuario.Login).FirstOrDefault();
                    if (usuarioDatabase == null)
                    {
                        var usuarioExterno = UsuarioExternoServico.Buscar(i => i.Login == usuario.Login).FirstOrDefault();
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
                                var perfil = new SistemaPerfil
                                {
                                    Excluido = false,
                                    Alteracao = DateTime.Now,
                                    CodigoSistema = origem.CodigoSistema,
                                    CodigoPerfil = item.Id,
                                    Origem = "I",
                                    UsuariosExternos = new List<UsuarioExterno> { usuarioExterno },
                                };

                                usuarioExterno.AdicionarPerfil(perfil);
                            }
                        }

                        UsuarioExternoServico.Salvar(usuarioExterno);
                        return;
                    }

                    foreach (var item in usuario.Perfis)
                    {
                        var perfil = new SistemaPerfil()
                        {
                            Excluido = false,
                            Alteracao = DateTime.Now,
                            CodigoSistema = origem.CodigoSistema,
                            CodigoPerfil = item.Id,
                            Origem = "I",
                        };

                        usuarioDatabase.AdicionarPerfil(perfil);
                    }

                    UsuarioServico.Salvar(usuarioDatabase);
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
            ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            StringBuilder endpoints = new StringBuilder();

            for (int i = 0; i < clientSection.Endpoints.Count; i++)
                endpoints.Append(clientSection.Endpoints[i].Address.OriginalString);

            StringBuilder sb = new StringBuilder();

            // TODO: Colocar essa rotina na biblioteca do Corporativo. 
            retorno.EHash = Hash(endpoints.ToString());
            retorno.ScHash = Hash(ConfigurationManager.ConnectionStrings[1].ConnectionString.ToString());

            try
            {
                retorno.Consulta = ((SistemaServico.Buscar(x => x.Excluido == false).Any()));
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
            StringBuilder sb = new StringBuilder();
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(conexaoBanco));
            }

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
        #endregion

        public void AtivarDesativarUsuarioExterno(string token, string login, bool excluido)
        {
            GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);
                    var usuario = UsuarioExternoServico.Buscar(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();
                    if (usuario != null)
                    {
                        usuario.AtivarDesativarTodosPerfis(origem.CodigoSistema, excluido);
                        UsuarioExternoServico.Salvar(usuario);
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

        public void AtivarDesativarUsuarioInterno(string token, string login, bool excluido)
        {
            GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                var loginBusca = login.PadRight(20);

                if (SistemaServico.ValidarToken(token))
                {
                    var origem = SistemaServico.DecomporToken(token);
                    var usuario = UsuarioServico.Buscar(x => x.Login.ToUpper().Equals(loginBusca.ToUpper())).FirstOrDefault();
                    if (usuario != null)
                    {
                        usuario.AtivarDesativarTodosPerfis(origem.CodigoSistema, excluido);
                        UsuarioServico.Salvar(usuario);
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
                var usuario = UsuarioServico.AutenticarUsuario(origem.CodigoSistema, login.UserName, login.Senha);
                usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);
                return Mapper.Map<ControleAcesso.Dominio.Entidades.Usuario, Usuario>(usuario);
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
                var usuario = UsuarioExternoServico.Autenticar(origem.CodigoSistema, login.UserName, login.Senha);
                usuario.PessoaFisica = _corporativo.PessoasFisicas.Get(usuario.IdPessoaFisica);


                return Mapper.Map<UsuarioExterno, Usuario>(usuario);
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
            var adHelper = new ActiveDirectoryHelper("");
            var user = adHelper.GetUserByLoginName(login);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public Usuario ObterUsuario(string token, string login)
        {
            GlobalContext.Properties["Request"] = "token=" + token;

            try
            {
                if (SistemaServico.ValidarToken(token))
                {
                    Usuario usuario = null;
                    var tipo = login.Contains("@") ? TipoUsuario.Externo : TipoUsuario.Interno;

                    if (tipo == TipoUsuario.Interno)
                    {
                        var usr = UsuarioServico.Buscar(u => u.Login.Trim().ToUpper() == login.Trim().ToUpper()).FirstOrDefault();
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
                        var usr = UsuarioExternoServico.Buscar(u => u.Login.Trim().ToUpper() == login.Trim().ToUpper()).FirstOrDefault();
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
    }



}