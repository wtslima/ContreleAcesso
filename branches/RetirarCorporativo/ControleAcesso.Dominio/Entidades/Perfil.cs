
namespace ControleAcesso.Dominio.Entidades
{
	public class Perfil : Controle
	{
		public virtual string Codigo { get; set; }
		public virtual string Nome { get; set; }
		
		public override int GetHashCode()
		{
			int hashCode = base.GetHashCode();
			unchecked {
				hashCode += 1000000007 * Id.GetHashCode();
				if (Nome != null) {
					hashCode += 1000000009 * Nome.GetHashCode();
				}
				
				if (Codigo != null) {
					hashCode += 1000000021 * Codigo.GetHashCode();
				}
			}
			
			return hashCode;
		}
		
		public override string ToString()
		{
			return string.Format("[Perfil Codigo={0}, Nome={1}]", Codigo, Nome);
		}
	}
}