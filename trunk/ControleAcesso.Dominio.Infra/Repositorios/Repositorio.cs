using System;
using System.Linq;
using ControleAcesso.Dominio.Entidades;
using NHibernate;
using NHibernate.Linq;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class Repositorio<T> : Corporativo.Infraestrutura.Repositorio.BaseRepositorio<T>
	{
		public Repositorio()
		{
			Conexao = new Corporativo.Infraestrutura.MsSql.Conexao("INMETRO", this.GetType().Assembly);
			Conexao.SchemaPadrao = "CONTROLEACESSO";
		}

        public override System.Collections.Generic.IEnumerable<T> Buscar(System.Linq.Expressions.Expression<Func<T, bool>> criterio)
        {
            var session = Conexao.ObterSessao(true);
            var t = session.Query<T>().Where(criterio);
            return t;
        }

		public override void SalvarComTransacao(T objeto)
		{
			if (objeto.GetType().GetInterface("IControle") != null) {
				(objeto as IControle).Alteracao = DateTime.Now;
			}

            //TODO: Verificar se há como remover o objeto das demais conexões ativas sem precisar fechá-las necessariamente.
            Conexao.FecharTodasAsSessoesAbertas();
            Conexao.LiberarObjetoDeSessoesAbertas(objeto);
            ISession session = Conexao.ObterSessao(true);
			using (ITransaction transaction = session.BeginTransaction())
			{
			    session.SaveOrUpdate(objeto);
			    transaction.Commit();
			}
		}
	}
}
