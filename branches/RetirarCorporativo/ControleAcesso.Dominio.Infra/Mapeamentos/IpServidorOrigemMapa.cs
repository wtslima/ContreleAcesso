using ControleAcesso.Dominio.ObjetosDeValor;

namespace ControleAcesso.Dominio.Infra.Mapeamentos
{
	public class ServidorOrigemMapa : ControleMapa<ServidorOrigem>
	{
		public ServidorOrigemMapa()
		{
            
			Table("TS_PARAMETRO");
			DynamicInsert();
			DynamicUpdate();
			CompositeId()
				.KeyProperty(x => x.CodigoSistema, "NOM_PARAMETRO")
				.KeyProperty(x => x.NomeParametro, "NOM_COMPLEMENTO_PARAMETRO");
			Map(x => x.Servidor, "CDA_VALOR_PARAMETRO_A").Length(100).Not.Nullable();
			Map(x => x.Token, "CDA_VALOR_PARAMETRO_B").Length(100);
			Where("NOM_COMPLEMENTO_PARAMETRO like 'IpServidorOrigem%'");
		}
	}
}
