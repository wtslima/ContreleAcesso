using System.Reflection;
using ControleAcesso.Dominio.Infra.Repositorios;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
	public class Servico<T> : BaseServico<T> where T : class
	{
	    private readonly Repositorio<T> _repositorio; 
        //protected Servico() : base() {
        //    Corporativo.Infraestrutura.Repositorio.Repositorio<> = new Repositorio<T>();
        //}
        //protected Servico() : this(Assembly.GetAssembly(typeof(Repositorio<T>))) {}

        //protected static Servico<T> _instancia;

        //public static Servico<T> Instancia
        //{
        //    get
        //    {
        //        if (_instancia == null)
        //        {
        //            _instancia = new Servico<T>(Assembly.GetAssembly(typeof(Repositorio<T>)));
        //        }

        //        return _instancia;
        //    }
        //}

        //public static Servico<T> NovaInstancia
        //{
        //    get
        //    {
        //        return _instancia = new Servico<T>(Assembly.GetAssembly(typeof(PerfilRepositorio)));
        //    }
        //}

	    public Servico(Repositorio<T> repositorio) : base(repositorio)
	    {
	        _repositorio = repositorio;
	    }

	    public new void SalvarComTransacao(T entidade) {
			_repositorio.SalvarComTransacao(entidade);
		}
	}
}