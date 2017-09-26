
using System;

namespace ControleAcesso.Dominio.Entidades
{
	/// <summary>
	/// Description of Controle.
	/// </summary>
	public abstract class Controle : Entidade, IControle
	{
		public virtual bool Excluido { get; set; }
		public virtual DateTime Alteracao { get; set; }
        public virtual string Origem { get; set; }

        public Controle()
        {
            Alteracao = DateTime.Now;
            Excluido = false;
            Origem = "I";
        }

		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Id.GetHashCode();
			}
			return hashCode;
		}
    }
}
