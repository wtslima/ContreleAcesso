using System.Reflection;
using ControleAcesso.Dominio.Infra.Repositorios;

namespace ControleAcesso.Dominio.Aplicacao.Servicos
{
	public class Servico<T> : Corporativo.Aplicacao.Servico.BaseServico<T> where T : class
	{
        protected Servico(Assembly assembly) : base(assembly) {
            Repositorio = new Repositorio<T>();
        }
        protected Servico() : this(Assembly.GetAssembly(typeof(Repositorio<T>))) {}

        protected static Servico<T> _instancia;

        public static Servico<T> Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Servico<T>(Assembly.GetAssembly(typeof(Repositorio<T>)));
                }

                return _instancia;
            }
        }

        public static Servico<T> NovaInstancia
        {
            get
            {
                return _instancia = new Servico<T>(Assembly.GetAssembly(typeof(PerfilRepositorio)));
            }
        }

		public override void SalvarComTransacao(T entidade) {
			Repositorio.SalvarComTransacao(entidade);
		}
	}
}