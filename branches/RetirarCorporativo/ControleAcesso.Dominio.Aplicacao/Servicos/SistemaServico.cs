using System;
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
	    public SistemaServico(ISistemaRepositorio repositorio) : base(repositorio)
	    {
	        _repositorio = repositorio;
	    }

	    public bool ValidarToken(string token)
		{
		    var servidor = DecomporToken(token);
			if (servidor == null) {
				throw new TokenInvalidoException(token);
			}
			
			return !string.IsNullOrWhiteSpace(servidor.CodigoSistema);
		}
		
		public ServidorOrigem DecomporToken(string token) {
		    try {
				var sistema = Buscar(s => s.ServidoresOrigem.Any(serv => serv.Token.Equals(token))).First();

				return sistema.ServidoresOrigem.FirstOrDefault(serv => serv.Token.Trim().Equals(token.Trim()));
			} catch (Exception ex) {
				throw new TokenInvalidoException(token, ex);
			}
		}
        
        public Sistema BuscarPorToken(string token) 
        {
        	return Buscar(s => s.ServidoresOrigem.Any(ip => ip.Token.Equals(token))).SingleOrDefault();
        }
		public void Excluir(string codigoSistema)
		{
		    var sistema = Buscar(s => s.Codigo.Trim().Equals(codigoSistema.Trim())).FirstOrDefault();
             Excluir(sistema);
		}

	    public void Cadastrar(Sistema objeto)
	    {
	        try
	        {
	            if (objeto != null)
	            {
	                var novoSistema = new Sistema
	                                  {
                                          Codigo = objeto.Codigo,
                                          Nome = objeto.Codigo
                                      };

                    _repositorio.Salvar(novoSistema);

	            }
               
	        }
	        catch
	        {
	            var sistema = 
                    Buscar(s => s.Codigo.ToUpper().Trim().Equals(objeto.Codigo.ToUpper().Trim())
                    && s.Ativo)
                    .FirstOrDefault();

	            if (sistema != null)
	            {
	                throw new SistemaCadastradoException();
	            }
	            throw;
	        }
	    }
	}
}
