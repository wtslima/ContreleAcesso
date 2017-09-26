
using System;

namespace ControleAcesso.Dominio.Entidades
{
	/// <summary>
	/// Description of IControle.
	/// </summary>
	public interface IControle
	{
		bool Excluido { get; set; }
		DateTime Alteracao { get; set; }
        string Origem { get; set; }
	}
}
