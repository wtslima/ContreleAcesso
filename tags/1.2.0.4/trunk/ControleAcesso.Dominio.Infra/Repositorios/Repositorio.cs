using System;
using ControleAcesso.Dominio.Entidades;
using NHibernate;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class Repositorio<T> : Corporativo.Infraestrutura.Repositorio.BaseRepositorio<T>
	{
		public Repositorio() : base()
		{
			Conexao = new Corporativo.Infraestrutura.MsSql.Conexao("INMETRO", this.GetType().Assembly, typeof(Corporativo.Dominio.Entidades.PessoaFisica).Assembly);
			Conexao.SchemaPadrao = "CONTROLEACESSO";
		}
		
		public override void SalvarComTransacao(T objeto)
		{
			if (objeto.GetType().GetInterface("IControle") != null) {
				(objeto as IControle).Alteracao = DateTime.Now;
			}

            //TODO: Verificar se há como remover o objeto das demais conexões ativas sem precisar fechá-las necessariamente.
            //Conexao.FecharTodasAsSessoesAbertas();
            //Conexao.LiberarObjetoDeSessoesAbertas(objeto);
			ISession session = this.Conexao.ObterSessao(true);
			using (ITransaction transaction = session.BeginTransaction())
			{
			session.SaveOrUpdate((object) objeto);
			transaction.Commit();
			}
		}
	}
}
