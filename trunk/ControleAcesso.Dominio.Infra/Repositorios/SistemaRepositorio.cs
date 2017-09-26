using ControleAcesso.Dominio.Entidades;

namespace ControleAcesso.Dominio.Infra.Repositorios
{
	public class SistemaRepositorio : Repositorio<Sistema>
	{
		public SistemaRepositorio()
		{
			_ordenarPor = "Nome";
		}
		
		public override void Salvar(Sistema objeto)
		{
			SalvarComTransacao(objeto);
		}
	}
}
