using System;
using System.Collections.Generic;
using System.Linq;
using ControleAcesso.Dominio.Aplicacao.Exceptions;
using ControleAcesso.Dominio.Aplicacao.Interfaces;
using ControleAcesso.Dominio.Entidades;
using ControleAcesso.Dominio.Interfaces.Repositorio;
using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
    /// <summary>
    /// Description of SistemaServico.
    /// </summary>
    public class SistemaServico : BaseServico<Sistema>, ISistemaServicoApp
    {
        private readonly ISistemaRepositorio _repositorio;
        
        public SistemaServico(ISistemaRepositorio repositorio)
            : base(repositorio)
        {
            _repositorio = repositorio;
        }

        public bool ValidarToken(string token)
        {
            var servidor = DecomporToken(token);
            if (servidor == null)
            {
                throw new TokenInvalidoException(token);
            }
            if (servidor.CodigoSistema > 0)
            {
                return true;
            }
            return false;
        }

        public ServidorOrigem DecomporToken(string token)
        {
            try
            {
                var sistema = Buscar(s => s.ServidoresOrigem.Any(serv => serv.Token.Equals(token))).First();

                return sistema.ServidoresOrigem.FirstOrDefault(serv => serv.Token.Trim().Equals(token.Trim()));
            }
            catch (Exception ex)
            {
                throw new TokenInvalidoException(token, ex);
            }
        }

        public Sistema BuscarPorToken(string token)
        {
            return Buscar(s => s.ServidoresOrigem.Any(ip => ip.Token.Equals(token))).SingleOrDefault();
        }

        public IEnumerable<Sistema> BuscarTodosSistemasPorCodigoPerfil(string codigoPerfil)
        {
            var sistemas = Buscar(s => s.PerfisAcesso.Any(ip => ip.Codigo.Equals(codigoPerfil)));
            return sistemas;
        }

        public IEnumerable<Sistema> BuscarPorCodigoOuDescricao(string codigoOuDescricao)
        {
            if (!string.IsNullOrWhiteSpace(codigoOuDescricao))
            {
                var sistemas = Buscar(x => x.Codigo.Trim().ToUpper().Contains(codigoOuDescricao.Trim().ToUpper()) || x.Nome.Trim().ToUpper().Contains(codigoOuDescricao.Trim().ToUpper()));
                return sistemas;
            }

            throw new Exception("Sistema não encontrado.");
        }

        public void Excluir(int id)
        {
            if (id > 0)
            {
                var sistema = Buscar(s => s.Id == id).FirstOrDefault();
                Excluir(sistema);
            }
            else
            {
                throw new Exception();
            }
        }

        public Sistema Cadastrar(Sistema objeto)
        {
            try
            {
                if (objeto.Id == 0)
                {
                    var novoSistema = new Sistema
                    {
                        Codigo = objeto.Codigo,
                        Nome = objeto.Nome
                    };
                    
                    return _repositorio.Cadastrar(novoSistema);

                }
                throw new SistemaCadastradoException();
            }
            catch(Exception ex)
            {
                    throw new Exception(ex.Message);
            }
        }

        public Sistema Atualizar(Sistema objeto)
        {
            try
            {
                if (objeto.Id > 0)
                {
                    return _repositorio.Atualizar(objeto);
                }
                throw new SistemaCadastradoException();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

     }
}
